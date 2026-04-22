using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface ISpecialtyRepository
    {
        Task<List<Speciality>> GetAllActiveAsync();
        Task<(List<Speciality> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? searchQuery);
        Task<Speciality?> GetByIdAsync(int id);
        Task<Speciality> AddAsync(Speciality specialty);
        Task UpdateAsync(Speciality specialty);
        Task DeleteAsync(Speciality specialty);
    }
}
