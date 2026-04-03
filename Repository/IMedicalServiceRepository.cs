using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IMedicalServiceRepository
    {
        Task<List<MedicalService>> GetAllActiveAsync();

        Task<MedicalService> GetByIdAsync(int id);
    }
}
