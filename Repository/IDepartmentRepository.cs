using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IDepartmentRepository
    {
        Task<Department?> GetByIdAsync(int id);

        Task<Department?> GetByNameAsync(string name);

        Task<List<Department>> GetAllAsync();
    }
}
