namespace FirstAidAPI.DTO.Admission
{
    /// <summary>
    /// Thông tin giường trống để y tá chọn đăng ký cho bệnh nhân.
    /// </summary>
    public class BedDto
    {
        public int Id { get; set; }
        public string BedNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int WardId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string WardType { get; set; } = string.Empty;
        public int Floor { get; set; }
    }
}
