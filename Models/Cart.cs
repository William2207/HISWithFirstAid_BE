using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAidAPI.Models
{
    public class Cart
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; } // Liên kết với User

        public DateOnly CreatedAt { get; set; }
        public DateOnly UpdatedAt { get; set; }

        // Navigation property
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}