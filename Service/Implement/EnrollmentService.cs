using AutoMapper;
using FirstAidAPI.DTO;
using FirstAidAPI.DTO.Enrollment;
using FirstAidAPI.Extensions;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;
using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.Service.Implement
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPracticalCourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EnrollmentService> _logger;

        public EnrollmentService(
        IEnrollmentRepository enrollmentRepository,
        IOrderRepository orderRepository,
        IPracticalCourseRepository courseRepository,
        IMapper mapper,
        ILogger<EnrollmentService> logger)
        {
            _enrollmentRepository = enrollmentRepository;
            _orderRepository = orderRepository;
            _courseRepository = courseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateEnrollmentsFromOrderAsync(int orderId)
        {
            _logger.LogInformation($"Creating enrollments for order {orderId}");

            var order = await _orderRepository.GetByIdWithItemsAsync(orderId);
            if (order == null)
            {
                throw new NotFoundException($"Order {orderId} not found");
            }

            foreach (var item in order.OrderItems)
            {
                // Kiểm tra đã đăng ký chưa
                var exists = await _enrollmentRepository.ExistsAsync(
                    order.UserId,
                    item.PracticalCourseId);

                if (!exists)
                {
                    var enrollment = new CourseEnrollment
                    {
                        UserId = order.UserId,
                        PracticalCourseId = item.PracticalCourseId,
                        OrderId = order.Id,
                        EnrolledAt = DateTime.UtcNow
                    };

                    await _enrollmentRepository.AddAsync(enrollment);

                    // Tăng số học viên
                    var course = await _courseRepository.GetByIdAsync(item.PracticalCourseId);
                    if (course != null)
                    {
                        course.EnrolledStudents++;
                        await _courseRepository.UpdateAsync(course);
                    }

                    _logger.LogInformation(
                        $"Created enrollment for user {order.UserId}, course {item.PracticalCourseId}");
                }
                else
                {
                    _logger.LogWarning(
                        $"User {order.UserId} already enrolled in course {item.PracticalCourseId}");
                }
            }
        }

        public async Task<bool> IsUserEnrolledAsync(int userId, int courseId)
        {
            return await _enrollmentRepository.ExistsAsync(userId, courseId);
        }

        public async Task<List<EnrollmentDto>> GetUserEnrollmentsAsync(int userId)
        {
            var enrollments = await _enrollmentRepository.GetByUserIdAsync(userId);
            return _mapper.Map<List<EnrollmentDto>>(enrollments);
        }

        public async Task<List<StudentDto>> GetCourseStudentsAsync(int courseId)
        {
            var enrollments = await _enrollmentRepository.GetByCourseIdAsync(courseId);
            return _mapper.Map<List<StudentDto>>(enrollments);
        }

        public async Task AddReviewAsync(int enrollmentId, int rating, string? review)
        {
            if (rating < 1 || rating > 5)
            {
                throw new ValidationException("Rating must be between 1 and 5");
            }

            var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId);
            if (enrollment == null)
            {
                throw new NotFoundException($"Enrollment {enrollmentId} not found");
            }

            enrollment.Rating = rating;
            enrollment.Review = review;
            enrollment.ReviewedAt = DateTime.UtcNow;

            await _enrollmentRepository.UpdateAsync(enrollment);

            _logger.LogInformation($"Added review for enrollment {enrollmentId}: {rating} stars");
        }

        public async Task<bool> ExistsAsync(int userId, int courseId)
        {
            return await _enrollmentRepository.ExistsAsync(userId, courseId);
        }

        public async Task<PagedResult<EnrollmentDto>> GetUserEnrollmentsAsync(int userId, int page, int pageSize)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID", nameof(userId));
            }

            // Lấy dữ liệu từ repository
            var pagedEnrollments = await _enrollmentRepository.GetUserEnrollmentsAsync(userId, page, pageSize);

            // Map từ CourseEnrollment sang EnrollmentDto
            var enrollmentDtos = pagedEnrollments.Data.Select(e => new EnrollmentDto
            {
                Id = e.Id,
                CourseId = e.PracticalCourseId,
                CourseName = e.PracticalCourse.Title,
                CourseStartDate = e.PracticalCourse.StartDate,
                CourseEndDate = e.PracticalCourse.EndDate,
                Location = e.PracticalCourse.Location,
                EnrolledAt = e.EnrolledAt,
                Rating = e.Rating
            }).ToList();
            // Tạo PagedResult mới với dữ liệu đã map
            return new PagedResult<EnrollmentDto>
            {
                Data = enrollmentDtos,
                CurrentPage = pagedEnrollments.CurrentPage,
                PageSize = pagedEnrollments.PageSize,
                TotalItems = pagedEnrollments.TotalItems,
                TotalPages = pagedEnrollments.TotalPages
            };
        }
    }
}