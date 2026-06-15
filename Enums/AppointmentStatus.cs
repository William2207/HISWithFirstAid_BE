namespace FirstAidAPI.Enums
{
    public enum AppointmentStatus
    {
        Registered = 0,  // Đã đăng ký nhưng chưa đến khám
        In_Progress = 1,   // Đang khám
        Completed = 2,     // Đã khám xong
        Cancelled = 3,
    }
}
