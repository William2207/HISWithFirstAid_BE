using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO.Cart
{
    public class UpdateCartItemRequestDto
    {
        [Required(ErrorMessage = "Cart Item ID is required")]
        public int CartItemId { get; set; }

        public int Quantity { get; set; }
    }
}