using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IInvoiceRepository
    {
        Task<Invoice> AddAsync(Invoice invoice);
        Task<Invoice?> GetByIdAsync(int id);
        Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber);
        Task<Invoice?> GetByAppointmentIdAsync(int appointmentId);
        Task<Invoice?> GetByLabOrderIdAsync(int labOrderId);
        Task<List<Invoice>> GetByPatientIdAsync(int patientId);
        Task<Invoice> UpdateAsync(Invoice invoice);
        Task<List<FirstAidAPI.DTO.Revenue.MonthlyRevenueDto>> GetMonthlyRevenueAsync(int year);
    }
}
