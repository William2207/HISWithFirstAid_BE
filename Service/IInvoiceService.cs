using FirstAidAPI.DTO.Invoice;

namespace FirstAidAPI.Service
{
    public interface IInvoiceService
    {
        Task<CreateInvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceDto createInvoiceDto);
    }
}
