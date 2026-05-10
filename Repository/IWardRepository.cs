using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IWardRepository
    {
        Task<List<Ward>> GetBySpecialtyAsync(int specialtyId);
    }
}
