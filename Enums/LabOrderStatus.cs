namespace FirstAidAPI.Enums
{
    public enum LabOrderStatus
    {
        Pending,      // Bác sĩ vừa tạo, chờ thanh toán
        Paid,         // Đã thanh toán, chờ thực hiện
        Completed,    // Đã có kết quả
        Cancelled
    }
}
