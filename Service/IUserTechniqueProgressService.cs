namespace FirstAidAPI.Service
{
    public interface IUserTechniqueProgressService
    {
        Task<bool> SaveCompletionProgressAsync(int userId, int techniqueId);
    }
}