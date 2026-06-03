namespace FirstAidAPI.DTO.Specialty
{
    public class SpecialtyDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public int DoctorCount { get; set; }
        public int? HeadDoctorId { get; set; }
        public string? HeadDoctorName { get; set; }
        public int? HeadNurseId { get; set; }
        public string? HeadNurseName { get; set; }
    }
}
