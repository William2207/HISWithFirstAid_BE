using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAidAPI.Models
{
    public class CourseEnrollment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int PracticalCourseId { get; set; }
        public int OrderId { get; set; }  // Link đến order đã thanh toán

        // Thông tin đăng ký
        public DateTime EnrolledAt { get; set; }  // Ngày đăng ký/thanh toán

        // Đánh giá sau khóa học (optional)
        public int? Rating { get; set; }  // Đánh giá 1-5 sao

        public string? Review { get; set; }  // Nhận xét
        public DateTime? ReviewedAt { get; set; }  // Ngày đánh giá

        // Navigation properties
        public User User { get; set; } = null!;

        public PracticalCourse PracticalCourse { get; set; } = null!;
        public Order Order { get; set; } = null!;
    }
}