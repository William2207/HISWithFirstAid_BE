namespace FirstAidAPI.Enums
{
    public enum PaymentMethod
    {
        CreditCard = 0,   // Thẻ tín dụng
        DebitCard = 1,    // Thẻ ghi nợ
        BankTransfer = 2, // Chuyển khoản
        Momo = 3,         // Ví Momo
        ZaloPay = 4,      // Ví ZaloPay
        VNPay = 5,        // VNPay
        COD = 6           // Thanh toán khi nhận hàng (nếu có)
    }
}