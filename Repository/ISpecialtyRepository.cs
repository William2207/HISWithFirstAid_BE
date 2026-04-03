using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface ISpecialtyRepository
    {
        Task<List<Speciality>> GetAllActiveAsync();

        Task<Speciality> GetByIdAsync(int id);
    }
}
