using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAidAPI.Models
{
    public class Patient
    {
        public int Id { get; set; }

        // ═══════════════════════════════════════
        // ⭐ UserId NULLABLE - Liên kết với User
        // ═══════════════════════════════════════
        public int? UserId { get; set; }

        // ═══════════════════════════════════════
        // ⭐ CHỈ LƯU KHI UserId = NULL (walk-in)
        // Nếu UserId != null → Lấy từ User
        // ═══════════════════════════════════════
        public string? FullName { get; set; }  // Nullable - chỉ cho walk-in

        public DateTime? DateOfBirth { get; set; }  // Nullable
        public string? Gender { get; set; }  // Nullable
        public string? PhoneNumber { get; set; }  // Nullable
        public string? Email { get; set; }  // Nullable
        public string? Address { get; set; }  // Nullable
        public string? IdCard { get; set; }  // Nullable

        // ═══════════════════════════════════════
        // THÔNG TIN Y TẾ (LUÔN LƯU Ở ĐÂY)
        // ═══════════════════════════════════════
        public string? InsuranceNumber { get; set; }  // BHYT

        public string? BloodType { get; set; }
        public string? Allergies { get; set; }
        public string? MedicalHistory { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContact { get; set; }
        public string? EmergencyContactRelationship { get; set; }

        // METADATA

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public User? User { get; set; }  // Nullable

        public List<Appointment> Appointments { get; set; } = new();
        public List<MedicalRecord> MedicalRecords { get; set; } = new();
        public List<Invoice> Invoices { get; set; } = new();

        [NotMapped]  // Không lưu vào DB
        public string FullNameDisplay => User?.FullName ?? FullName ?? "Unknown";

        [NotMapped]
        public DateTime? DateOfBirthDisplay => User?.DateOfBirth ?? DateOfBirth;

        [NotMapped]
        public string GenderDisplay => User?.Gender ?? Gender ?? "Unknown";

        [NotMapped]
        public string PhoneNumberDisplay => User?.PhoneNumber ?? PhoneNumber ?? "N/A";

        [NotMapped]
        public string EmailDisplay => User?.Email ?? Email ?? "N/A";
    }
}
