using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Invoice?> GetByIdAsync(int id)
        {
            return await _context.Invoices
                .Include(i => i.Items)
                .Include(i => i.Patient)
                .Include(i => i.Appointment)
                .Include(i => i.Payments)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            return await _context.Invoices
                .Include(i => i.Items)
                .Include(i => i.Patient)
                .Include(i => i.Appointment)
                .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
        }

        public async Task<Invoice?> GetByAppointmentIdAsync(int appointmentId)
        {
            return await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.AppointmentId == appointmentId);
        }

        public async Task<List<Invoice>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Invoices
                .Include(i => i.Items)
                .Include(i => i.Appointment)
                .Include(i => i.Payments)
                .Where(i => i.PatientId == patientId)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
        }

        public async Task<Invoice> UpdateAsync(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }
    }
}
