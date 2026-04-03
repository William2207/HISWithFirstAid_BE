using FirstAidAPI.DTO.Specialty;
using FirstAidAPI.Repository;

namespace FirstAidAPI.Service.Implement
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly ISpecialtyRepository _specialtyRepository;

        public SpecialtyService(ISpecialtyRepository specialtyRepository)
        {
            _specialtyRepository = specialtyRepository;
        }

        public async Task<List<SpecialtyLookupDTO>> GetSpecialtiesForLookupAsync()
        {
            var specialties = await _specialtyRepository.GetAllActiveAsync();
            return specialties.Select(s => new SpecialtyLookupDTO
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description
            }).ToList();
        }
    }
}
