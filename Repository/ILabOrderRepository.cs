using FirstAidAPI.DTO.LabOrder;
using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface ILabOrderRepository
    {
        Task<LabOrder?> GetByIdAsync(int id);
        Task<List<LabOrder>> GetByAppointmentIdAsync(int appointmentId);

        /// <summary>Lấy danh sách LabOrder Pending chưa có Invoice — cho Receptionist</summary>
        Task<List<LabOrder>> GetPendingWithNoInvoiceAsync();

        Task<LabOrder> AddAsync(LabOrder labOrder);
        Task<LabOrder> UpdateAsync(LabOrder labOrder);
    }
}
