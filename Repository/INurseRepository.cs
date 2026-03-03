using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface INurseRepository
    {
        Task<Nurse> AddAsync(Nurse nurse);

        Task UpdateAsync(Nurse nurse);

        Task DeleteAsync(int id);

        Task<Nurse?> GetByIdAsync(int id);

        Task<Nurse?> GetByUserIdAsync(int userId);

        Task<List<Nurse>> GetAllAsync();

        Task<bool> ExistsByUserIdAsync(int userId);
    }
}
