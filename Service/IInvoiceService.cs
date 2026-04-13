using FirstAidAPI.DTO.Invoice;
using FirstAidAPI.DTO.Payment;

namespace FirstAidAPI.Service
{
    public interface IInvoiceService
    {
        Task<CreateInvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceDto createInvoiceDto);

        Task CompleteInvoiceAsync(int invoiceId, string transactionId);

        Task FailInvoiceAsync(int invoiceId);

        Task<CreateInvoiceResponseDto?> GetByInvoiceNumberAsync(string invoiceNumber);
        
        Task<IEnumerable<PatientInvoiceDto>> GetInvoicesByPatientAsync(int patientId);

        Task<IEnumerable<PatientInvoiceDto>> GetInvoicesByUserIdAsync(int userId);
        
        Task ProcessMomoPaymentAsync(MomoCallbackDto callback);
    }
}
