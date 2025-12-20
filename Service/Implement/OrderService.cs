using AutoMapper;
using FirstAidAPI.Controllers;
using FirstAidAPI.DTO.Order;
using FirstAidAPI.DTO.Payment;
using FirstAidAPI.Enums;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;
using FirstAidAPI.Repository.Implement;
using FirstAidAPI.Service.Payment;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using FirstAidAPI.Exceptions;

namespace FirstAidAPI.Service.Implement
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IPracticalCourseRepository _courseRepository;
        private readonly IMomoService _momoService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IPracticalCourseRepository practicalCourseRepository, IMomoService momoService, IEnrollmentService enrollmentService, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _courseRepository = practicalCourseRepository;
            _momoService = momoService;
            _enrollmentService = enrollmentService;
            _logger = logger;
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

                //Kiem tra trung lap cung khoa hoc + thoi gian
                var userEnrollments = await _enrollmentService.GetUserEnrollmentsAsync(createOrderDto.UserId);
                foreach (var newCourse in courses)
                {
                    var duplicateEnrollment = userEnrollments.FirstOrDefault(ec => ec.CourseId == newCourse.Id && ec.CourseStartDate == newCourse.StartDate && ec.CourseEndDate == newCourse.EndDate);
                    if (duplicateEnrollment != null)
                    {
                        throw new BusinessException(
                            $"Bạn đã đăng ký khóa học '{newCourse.Title}' " +
                            $"(lớp từ {newCourse.StartDate:dd/MM/yyyy} đến {newCourse.EndDate:dd/MM/yyyy}) rồi.");
                    }
                }

                // KIỂM TRA XUNG ĐỘT LỊCH HỌC (giữa tất cả các khóa)
                // Kiểm tra khóa mới với các khóa đã đăng ký
                foreach (var newCourse in courses)
                {
                    foreach (var enrolledCourse in userEnrollments)
                    {
                        if (enrolledCourse.CourseId == newCourse.Id)
                            continue;

                        // Kiểm tra xung đột thời gian với các khóa học KHÁC
                        bool hasTimeConflict = IsDateRangeOverlap(
                            newCourse.StartDate, newCourse.EndDate,
                            enrolledCourse.CourseStartDate, enrolledCourse.CourseEndDate);
                    }
                }
                //KIỂM TRA XUNG ĐỘT GIỮA CÁC KHÓA TRONG ĐƠN HÀNG
                for (int i = 0; i < courses.Count; i++)
                {
                    for (int j = i + 1; j < courses.Count; j++)
                    {
                        if (courses[i].Id == courses[j].Id && courses[i].StartDate == courses[j].StartDate && courses[i].EndDate == courses[j].EndDate)
                        {
                            throw new BusinessException(
                                 $"Khóa học '{courses[i].Title}' " +
                                 $"(lớp từ {courses[i].StartDate:dd/MM/yyyy} đến {courses[i].EndDate:dd/MM/yyyy}) " +
                                 $"xuất hiện nhiều lần trong đơn hàng.");
                        }

                        if (courses[i].Id != courses[j].Id)
                        {
                            bool hasTimeConflict = IsDateRangeOverlap(
                                courses[i].StartDate, courses[i].EndDate,
                                courses[j].StartDate, courses[j].EndDate);
                            if (hasTimeConflict)
                            {
                                throw new BusinessException(
                                    $"Khóa học '{courses[i].Title}' " +
                                    $"(lớp từ {courses[i].StartDate:dd/MM/yyyy} đến {courses[i].EndDate:dd/MM/yyyy}) " +
                                    $"xung đột lịch học với khóa học '{courses[j].Title}' " +
                                    $"(lớp từ {courses[j].StartDate:dd/MM/yyyy} đến {courses[j].EndDate:dd/MM/yyyy}).");
                            }
                        }
                    }
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

                foreach (var item in order.OrderItems)
                {
                    var alreadyEnrolled = await _enrollmentService.ExistsAsync(
                        order.UserId,
                        item.PracticalCourseId);

                    if (alreadyEnrolled)
                    {
                        _logger.LogWarning(
                            $"User {order.UserId} already enrolled in course {item.PracticalCourseId}. Marking order as failed.");

                        // Đánh dấu order là Failed thay vì Completed
                        order.PaymentStatus = PaymentStatus.Failed;
                        order.OrderStatus = OrderStatus.Cancelled;
                        order.TransactionId = transactionId;
                        await _orderRepository.UpdateAsync(order);

                        scope.Complete();

                        throw new BusinessException(
                            "Không thể hoàn tất đơn hàng: Bạn đã đăng ký một số khóa học trong đơn hàng này rồi. " +
                            "Vui lòng liên hệ support để được hoàn tiền.");
                    }
                }

                order.PaymentStatus = PaymentStatus.Completed;
                order.OrderStatus = OrderStatus.Completed;
                order.CompletedAt = DateTime.UtcNow;
                order.TransactionId = transactionId;
                await _orderRepository.UpdateAsync(order);
                await _enrollmentService.CreateEnrollmentsFromOrderAsync(orderId);

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

        private bool IsDateRangeOverlap(DateOnly start1, DateOnly end1, DateOnly start2, DateOnly end2)
        {
            return start1 <= end2 && start2 <= end1;
        }
    }
}