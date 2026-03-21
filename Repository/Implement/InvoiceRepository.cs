using FirstAidAPI.Data;
using FirstAidAPI.Models;

namespace FirstAidAPI.Repository.Implement
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly FirstAidContext _context;

        public InvoiceRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<Invoice> AddAsync(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }
    }
}
