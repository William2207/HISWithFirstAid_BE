namespace FirstAidAPI.DTO.Queue
{
    public class PrintTicketDto
    {
        public int QueueNumber { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime PrintTime { get; set; }
    }
}
