using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IClinicRepository
    {
        Task<List<Clinic>> GetAllAsync();

        Task<Clinic?> GetByIdAsync(int id);

        Task<List<Clinic>> GetBySpecialtyAsync(int specialtyId);
    }
}
