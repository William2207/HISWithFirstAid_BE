using FirstAidAPI.DTO.Invoice;

namespace FirstAidAPI.Service
{
    public interface IInvoiceService
    {
        Task<CreateInvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceDto createInvoiceDto);

        Task CompleteInvoiceAsync(int invoiceId, string transactionId);

        Task FailInvoiceAsync(int invoiceId);

        Task<CreateInvoiceResponseDto?> GetByInvoiceNumberAsync(string invoiceNumber);
    }
}
