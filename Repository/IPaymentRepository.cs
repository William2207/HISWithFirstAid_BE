using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IPaymentRepository
    {
        Task<Payment> AddAsync(Payment payment);
        Task<Payment?> GetByTransactionIdAsync(string transactionId);
        Task<List<Payment>> GetByInvoiceIdAsync(int invoiceId);
    }
}
