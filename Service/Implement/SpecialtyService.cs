using FirstAidAPI.DTO.Specialty;
using FirstAidAPI.Models;
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
            return specialties.Select(MapToLookupDto).ToList();
        }

        public async Task<(List<SpecialtyDTO> Items, int TotalCount)> GetPagedSpecialtiesAsync(
            int page, int pageSize, string? searchQuery)
        {
            var (items, totalCount) = await _specialtyRepository.GetPagedAsync(page, pageSize, searchQuery);
            var dtos = items.Select(MapToDto).ToList();
            return (dtos, totalCount);
        }

        public async Task<SpecialtyDTO> GetSpecialtyByIdAsync(int id)
        {
            var specialty = await _specialtyRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Không tìm thấy chuyên khoa với Id = {id}.");
            return MapToDto(specialty);
        }

        public async Task<SpecialtyDTO> CreateSpecialtyAsync(CreateSpecialtyRequest request)
        {
            var specialty = new Speciality
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                IsActive = true
            };

            var created = await _specialtyRepository.AddAsync(specialty);
            return MapToDto(created);
        }

        public async Task<SpecialtyDTO> UpdateSpecialtyAsync(int id, UpdateSpecialtyRequest request)
        {
            var specialty = await _specialtyRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Không tìm thấy chuyên khoa với Id = {id}.");

            specialty.Name = request.Name;
            specialty.Description = request.Description;
            specialty.Price = request.Price;
            specialty.IsActive = request.IsActive;

            await _specialtyRepository.UpdateAsync(specialty);
            return MapToDto(specialty);
        }

        public async Task DeleteSpecialtyAsync(int id)
        {
            var specialty = await _specialtyRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Không tìm thấy chuyên khoa với Id = {id}.");

            await _specialtyRepository.DeleteAsync(specialty);
        }

        // --- Private helpers ---

        private static SpecialtyDTO MapToDto(Speciality specialty) => new()
        {
            Id = specialty.Id,
            Name = specialty.Name,
            Description = specialty.Description,
            Price = specialty.Price,
            IsActive = specialty.IsActive,
            DoctorCount = specialty.Doctors?.Count ?? 0
        };

        private static SpecialtyLookupDTO MapToLookupDto(Speciality specialty) => new()
        {
            Id = specialty.Id,
            Name = specialty.Name,
            Description = specialty.Description,
            Price = specialty.Price
        };
    }
}
