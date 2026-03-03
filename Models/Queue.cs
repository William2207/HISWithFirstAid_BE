namespace FirstAidAPI.Models
{
    public class Queue
    {
        public int Id { get; set; }

        // Số thứ tự
        public int QueueNumber { get; set; }

        // Thời gian
        public DateTime IssueTime { get; set; } = DateTime.UtcNow;  // Thời điểm lấy số

        public DateTime? CalledTime { get; set; }  // Thời điểm gọi
        public DateTime? CompletedTime { get; set; }  // Thời điểm hoàn tất

        // Trạng thái
        public string Status { get; set; } = "WAITING";

        public DateOnly QueueDate { get; set; }

        // "WAITING"    - Đang chờ
        // "CALLED"     - Đã gọi
        // "COMPLETED"  - Hoàn tất (bệnh nhân đã nhận phiếu)
        // "CANCELLED"  - Hủy (không đến)
    }
}
