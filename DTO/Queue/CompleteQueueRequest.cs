namespace FirstAidAPI.DTO.Queue
{
    public class CompleteQueueRequest
    {
        public string PatientName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
    }
}
