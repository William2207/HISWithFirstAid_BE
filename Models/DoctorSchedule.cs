using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAidAPI.Models
{
    [Table("DoctorSchedule")]
    public class DoctorSchedule
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public bool IsNightShift { get; set; } = false;
        public DateOnly Date { get; set; }
        public bool IsOff { get; set; } = false;

        // Ca ngày → có ClinicId, không có SpecialtyId
        public int? ClinicId { get; set; }

        // Ca đêm → có SpecialtyId, không có ClinicId
        public int? SpecialtyId { get; set; }

        public int? WardId { get; set; }

        public int MaxOnlineSlots { get; set; } = 10;
        public int MaxWalkInSlots { get; set; } = 10;

        // Navigation
        public Doctor Doctor { get; set; } = null!;

        public Clinic? Clinic { get; set; }
        public Speciality? Specialty { get; set; }
        public Ward? Ward { get; set; }
    }
}
