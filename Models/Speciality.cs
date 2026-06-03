namespace FirstAidAPI.Models
{
    public class Speciality
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal Price { get; set; }
        public int? HeadDoctorId { get; set; }
        public int? HeadNurseId { get; set; }

        // Navigation
        public Doctor? HeadDoctor { get; set; }

        public Nurse? HeadNurse { get; set; }

        public List<Doctor> Doctors { get; set; } = new();
        public List<Nurse> Nurses { get; set; } = new();

        public List<Appointment> Appointments { get; set; } = new();
    }
}
