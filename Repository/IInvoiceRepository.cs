using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IInvoiceRepository
    {
        Task<Invoice> AddAsync(Invoice invoice);
    }
}
