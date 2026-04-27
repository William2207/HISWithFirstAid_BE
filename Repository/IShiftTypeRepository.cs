using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IShiftTypeRepository
    {
        Task<List<ShiftType>> GetAllAsync();

        Task<ShiftType?> GetByIdAsync(int id);

        Task<ShiftType?> GetNightShiftAsync();
    }
}
