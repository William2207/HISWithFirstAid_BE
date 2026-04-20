using FirstAidAPI.DTO.Nurse;

namespace FirstAidAPI.Service
{
    public interface INurseService
    {
        Task<NurseProfileDto?> GetNurseProfileAsync(int userId);
        Task<bool> UpdateNurseProfileAsync(int userId, UpdateNurseProfileDto updateDto);
    }
}
