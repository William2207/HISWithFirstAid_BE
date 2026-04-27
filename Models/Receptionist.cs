using FirstAidAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace FirstAidAPI.Models
{
    public class Receptionist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? WorkStation { get; set; }
        public bool IsAvailable { get; set; } = true;

        public User User { get; set; } = null!;

        //public List<Payment> PaymentsCollected { get; set; } = new();
    }
}
