using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAidAPI.Models
{
    [Table("NurseSchedule")]
    public class NurseSchedule
    {
        public int Id { get; set; }
        public int NurseId { get; set; }
        public bool IsNightShift { get; set; } = false;
        public DateOnly Date { get; set; }
        public bool IsOff { get; set; } = false;

        // Ca ngày -> có WardId, không có SpecialtyId
        public int? WardId { get; set; }

        // Ca đêm -> có SpecialtyId, không có WardId
        public int? SpecialtyId { get; set; }

        // Navigation
        public Nurse Nurse { get; set; } = null!;
        public Ward? Ward { get; set; }
        public Speciality? Specialty { get; set; }
    }
}
