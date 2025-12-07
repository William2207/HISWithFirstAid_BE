using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO.Cart
{
    public class AddCartItemRequestDto
    {
        [Required(ErrorMessage = "Course ID is required")]
        public int PracticalCourseId { get; set; }

        public int Quantity { get; set; } = 1;
    }
}