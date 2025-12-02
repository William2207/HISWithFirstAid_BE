using AutoMapper;
using FirstAidAPI.DTO.Order;
using FirstAidAPI.DTO.Payment;
using FirstAidAPI.Enums;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;
using FirstAidAPI.Service.Payment;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace FirstAidAPI.Service.Implement
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IPracticalCourseRepository _courseRepository;
        private readonly IMomoService _momoService;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IPracticalCourseRepository practicalCourseRepository, IMomoService momoService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _courseRepository = practicalCourseRepository;
            _momoService = momoService;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto?> GetByOrderNumberAsync(string orderNumber)
        {
            var order = await _orderRepository.GetByOrderNumberAsync(orderNumber);
            if (order == null)
            {
                return null;
            }
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<CreateOrderResponseDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            using var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.Serializable
                },
                TransactionScopeAsyncFlowOption.Enabled
            );

            try
            {
                // 1. Lock và validate courses (Pessimistic Locking)
                var courseIds = createOrderDto.Items.Select(i => i.PracticalCourseId).ToList();
                var courses = await _courseRepository.GetByIdsWithLockAsync(courseIds);

                if (courses.Count != createOrderDto.Items.Count)
                {
                    throw new Exception("Một số khóa học không tồn tại.");
                }

                // 2. Kiểm tra slot còn trống
                foreach (var course in courses)
                {
                    if (course.EnrolledStudents >= course.MaxStudents)
                    {
                        throw new Exception($"Khóa học '{course.Title}' đã hết chỗ.");
                    }
                }
                // 4. Tính tổng tiền
                decimal totalAmount = courses.Sum(c => c.Price);

                // 5. Tạo Order (Pending - chưa tăng EnrolledStudents)
                var order = _mapper.Map<Order>(createOrderDto);
                order.TotalAmount = totalAmount;
                order.OrderNumber = GenerateOrderNumber();
                order.PaymentMethod = PaymentMethod.Momo;
                order.PaymentStatus = PaymentStatus.Pending;
                order.OrderStatus = OrderStatus.Pending;
                order.CreatedAt = DateTime.UtcNow;

                var createdOrder = await _orderRepository.CreateAsync(order);

                // 6. Commit transaction trước khi gọi Momo
                scope.Complete();

                // 7. Tạo payment URL Momo (ngoài transaction)
                var momoResponse = await _momoService.CreatePaymentAsync(new MomoCreatePaymentRequestDto
                {
                    OrderNumber = createdOrder.OrderNumber,
                    Amount = (long)totalAmount,
                    OrderDescription = $"Thanh toán đơn hàng {createdOrder.OrderNumber}"
                });

                if (momoResponse.ResultCode != 0)
                {
                    throw new Exception($"Lỗi tạo thanh toán Momo: {momoResponse.Message}");
                }

                return new CreateOrderResponseDto
                {
                    Order = _mapper.Map<OrderDto>(createdOrder),
                    PaymentUrl = momoResponse.PayUrl
                };
            }
            catch (Exception)
            {
                scope.Dispose();
                throw;
            }
        }

        private string GenerateOrderNumber()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var random = new Random().Next(100, 999);
            return $"ORD{timestamp}{random}";
        }

        public async Task CompleteOrderAsync(int orderId, string transactionId)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                {
                    throw new Exception("Order not found");
                }

                // 1. Cập nhật Order
                order.PaymentStatus = PaymentStatus.Completed;
                order.OrderStatus = OrderStatus.Completed;
                order.CompletedAt = DateTime.UtcNow;
                order.TransactionId = transactionId;
                await _orderRepository.UpdateAsync(order);

                // 2. Tăng EnrolledStudents
                var courseIds = order.OrderItems.Select(i => i.PracticalCourseId).ToList();
                var courses = await _courseRepository.GetByIdsAsync(courseIds);

                foreach (var course in courses)
                {
                    course.EnrolledStudents += 1;
                    await _courseRepository.UpdateAsync(course);
                }

                // 3. Cấp quyền truy cập khóa học
                //await GrantCourseAccessAsync(order);

                scope.Complete();
            }
            catch (Exception)
            {
                scope.Dispose();
                throw;
            }
        }

        public async Task FailOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            order.PaymentStatus = PaymentStatus.Failed;
            order.OrderStatus = OrderStatus.Cancelled;
            await _orderRepository.UpdateAsync(order);
        }

        //private async Task GrantCourseAccessAsync(Order order)
        //{
        //    var enrollments = order.OrderItems.Select(item => new CourseEnrollment
        //    {
        //        UserId = order.UserId,
        //        CourseId = item.PracticalCourseId,
        //        OrderId = order.Id,
        //        EnrolledAt = DateTime.UtcNow,
        //        IsActive = true
        //    }).ToList();

        //    await _enrollmentRepository.AddRangeAsync(enrollments);
        //}
    }
}