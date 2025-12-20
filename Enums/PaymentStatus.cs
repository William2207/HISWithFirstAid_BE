namespace FirstAidAPI.Enums
{
    public enum PaymentStatus
    {
        Pending = 0,      // Chờ thanh toán
        Processing = 1,   // Đang xử lý
        Completed = 2,    // Thanh toán thành công
        Failed = 3,       // Thanh toán thất bại
        Refunded = 4,     // Đã hoàn tiền
        Cancelled = 5     // Đã hủy
    }
}