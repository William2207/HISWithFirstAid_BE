namespace FirstAidAPI.DTO.Doctor
{
    public class DoctorLookupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public int SpecialtyId { get; set; }
        public int? YearsOfExperience { get; set; }
    }
}
