using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IPatientRepository
    {
        Task<Patient> AddAsync(Patient patient);

        Task UpdateAsync(Patient patient);

        Task DeleteAsync(int id);

        Task<Patient?> GetByIdAsync(int id);

        Task<Patient?> GetByUserIdAsync(int userId);

        Task<List<Patient>> GetAllAsync();
        Task<List<Patient>> GetPatientsBySpecialtyAsync(int specialtyId);
        Task<bool> ExistsByUserIdAsync(int userId);

        Task<Patient?> GetByIdCardAsync(string idCard);
    }
}
