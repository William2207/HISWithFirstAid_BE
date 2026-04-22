namespace FirstAidAPI.Models
{
    public class Speciality
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal Price { get; set; }

        // Navigation
        public List<Doctor> Doctors { get; set; } = new();

        public List<Appointment> Appointments { get; set; } = new();
    }
}
