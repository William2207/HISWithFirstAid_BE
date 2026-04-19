using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IReceptionistRepository
    {
        Task<Receptionist> AddAsync(Receptionist receptionist);

        Task UpdateAsync(Receptionist receptionist);

        Task DeleteAsync(int id);

        Task<Receptionist?> GetByIdAsync(int id);

        Task<Receptionist?> GetByUserIdAsync(int userId);

        Task<List<Receptionist>> GetAllAsync();

        Task<bool> ExistsByUserIdAsync(int userId);
    }
}
