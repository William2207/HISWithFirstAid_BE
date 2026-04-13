using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IInvoiceRepository
    {
        Task<Invoice> AddAsync(Invoice invoice);
        Task<Invoice?> GetByIdAsync(int id);
        Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber);
        Task<Invoice?> GetByAppointmentIdAsync(int appointmentId);
        Task<List<Invoice>> GetByPatientIdAsync(int patientId);
        Task<Invoice> UpdateAsync(Invoice invoice);
    }
}
