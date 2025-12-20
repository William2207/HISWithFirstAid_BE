using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface ISavedTechniqueRepository
    {
        Task<SavedTechnique> AddSavedTechniqueAsync(SavedTechnique savedTechnique);

        Task<bool> RemoveSavedTechniqueAsync(int userId, int techniqueId);

        Task<IEnumerable<SavedTechnique>> GetSavedTechniquesByUserAsync(int userId);
    }
}