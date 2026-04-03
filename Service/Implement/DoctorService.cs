using FirstAidAPI.DTO.Doctor;
using FirstAidAPI.Repository;

namespace FirstAidAPI.Service.Implement
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<List<DoctorLookupDTO>> GetDoctorsForLookupAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return doctors
                .Where(d => d.IsAvailable)
                .Select(d => new DoctorLookupDTO
                {
                    Id = d.Id,
                    Name = d.User?.FullName ?? string.Empty,
                    Specialty = d.PrimarySpecialty?.Name ?? string.Empty,
                    YearsOfExperience = d.YearsOfExperience
                })
                .ToList();
        }
    }
}
