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
                .Include(i => i.Patient)
                .Include(i => i.Appointment)
                .Include(i => i.Payments)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            return await _context.Invoices
                .Include(i => i.Patient)
                .Include(i => i.Appointment)
                .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
        }

        public async Task<Invoice?> GetByAppointmentIdAsync(int appointmentId)
        {
            return await _context.Invoices
                .FirstOrDefaultAsync(i => i.AppointmentId == appointmentId);
        }

        public async Task<Invoice?> GetByLabOrderIdAsync(int labOrderId)
        {
            return await _context.Invoices
                .FirstOrDefaultAsync(i => i.LabOrderId == labOrderId);
        }

        public async Task<List<Invoice>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Invoices
                .Include(i => i.Appointment)
                    .ThenInclude(a => a!.Specialty)
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

        public async Task<List<FirstAidAPI.DTO.Revenue.MonthlyRevenueDto>> GetMonthlyRevenueAsync(int year)
        {
            var monthlyData = await _context.Invoices
                .Where(i => i.Status == Enums.OrderStatus.Completed
                            && i.PaidAt.HasValue
                            && i.PaidAt.Value.Year == year)
                .GroupBy(i => i.PaidAt!.Value.Month)
                .Select(g => new FirstAidAPI.DTO.Revenue.MonthlyRevenueDto
                {
                    Month = g.Key,
                    HospitalRevenue = g.Sum(i => i.PaidAmount)
                })
                .OrderBy(x => x.Month)
                .ToListAsync();

            var allMonths = Enumerable.Range(1, 12).Select(month => new FirstAidAPI.DTO.Revenue.MonthlyRevenueDto
            {
                Month = month,
                MonthName = new DateTime(year, month, 1).ToString("MMMM"),
                HospitalRevenue = monthlyData.FirstOrDefault(m => m.Month == month)?.HospitalRevenue ?? 0
            }).ToList();

            return allMonths;
        }
    }
}
