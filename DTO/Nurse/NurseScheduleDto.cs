namespace FirstAidAPI.DTO.Nurse
{
    public class NurseScheduleDto
    {
        public int NurseId { get; set; }
        public string NurseName { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public string? ShiftName { get; set; }
        public string? WardRoom { get; set; }
        public int? SpecialtyId { get; set; }
        public bool IsOff { get; set; }
    }
}
