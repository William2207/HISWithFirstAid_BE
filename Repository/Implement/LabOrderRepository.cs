using FirstAidAPI.Data;
using FirstAidAPI.Enums;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class LabOrderRepository : ILabOrderRepository
    {
        private readonly FirstAidContext _context;

        public LabOrderRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<LabOrder?> GetByIdAsync(int id)
        {
            return await _context.LabOrders
                .Include(lo => lo.Items)
                    .ThenInclude(li => li.MedicalService)
                .Include(lo => lo.Appointment)
                    .ThenInclude(a => a.Patient)
                        .ThenInclude(p => p.User)
                .Include(lo => lo.Appointment)
                    .ThenInclude(a => a.Doctor)
                        .ThenInclude(d => d.User)
                .FirstOrDefaultAsync(lo => lo.Id == id);
        }

        public async Task<List<LabOrder>> GetByAppointmentIdAsync(int appointmentId)
        {
            return await _context.LabOrders
                .Include(lo => lo.Items)
                    .ThenInclude(li => li.MedicalService)
                .Where(lo => lo.AppointmentId == appointmentId)
                .OrderByDescending(lo => lo.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<LabOrder>> GetPendingWithNoInvoiceAsync()
        {
            return await _context.LabOrders
                .Include(lo => lo.Items)
                    .ThenInclude(li => li.MedicalService)
                .Include(lo => lo.Appointment)
                    .ThenInclude(a => a.Patient)
                        .ThenInclude(p => p.User)
                .Include(lo => lo.Appointment)
                    .ThenInclude(a => a.Doctor)
                        .ThenInclude(d => d.User)
                .Where(lo => lo.Status == LabOrderStatus.Pending
                          && !_context.Invoices.Any(i => i.LabOrderId == lo.Id))
                .OrderByDescending(lo => lo.CreatedAt)
                .ToListAsync();
        }

        public async Task<LabOrder> AddAsync(LabOrder labOrder)
        {
            _context.LabOrders.Add(labOrder);
            await _context.SaveChangesAsync();
            return labOrder;
        }

        public async Task<LabOrder> UpdateAsync(LabOrder labOrder)
        {
            _context.LabOrders.Update(labOrder);
            await _context.SaveChangesAsync();
            return labOrder;
        }
    }
}
