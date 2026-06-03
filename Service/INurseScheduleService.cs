using FirstAidAPI.DTO.Nurse;

namespace FirstAidAPI.Service
{
    public interface INurseScheduleService
    {
        Task GenerateMonthlyScheduleAsync(int month, int year);
        Task GenerateSpecialtyScheduleAsync(int specialtyId, int month, int year);
        Task<List<NurseScheduleDto>> GetMonthlyScheduleAsync(int month, int year);
        Task<List<NurseScheduleDto>> GetNurseScheduleAsync(int nurseId, int month, int year);
        Task<List<NurseScheduleDto>> GetSpecialtyScheduleAsync(int specialtyId, int month, int year);
    }
}
