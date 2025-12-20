using FirstAidAPI.DTO.SavedTechniques;

namespace FirstAidAPI.Service
{
    public interface ISavedTechniqueService
    {
        Task<IEnumerable<SavedTechniqueNewDto>> GetSavedTechniquesByUserAsync(int userId);

        Task<SavedTechniqueNewDto> AddSavedTechniqueAsync(AddSavedTechniqueDto addSavedTechniqueDto, int userId);

        Task<bool> RemoveSavedTechniqueAsync(int userId, int techniqueId);
    }
}