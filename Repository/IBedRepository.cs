using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IBedRepository
    {
        Task<List<Bed>> GetAvailableBedsAsync();
        Task<Bed?> GetByIdAsync(int id);
        Task UpdateAsync(Bed bed);
    }
}
