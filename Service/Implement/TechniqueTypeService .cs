using AutoMapper;
using FirstAidAPI.DTO;
using FirstAidAPI.DTO.Technique;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;

namespace FirstAidAPI.Service.Implement
{
    public class TechniqueTypeService : ITechniqueTypeService
    {
        private readonly ITechniqueTypeRepository _repository;
        private readonly IMapper _mapper;

        public TechniqueTypeService(ITechniqueTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TechniqueTypeDto>> GetAllAsync()
        {
            var techniqueTypes = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TechniqueTypeDto>>(techniqueTypes);
        }

        public async Task<TechniqueTypeDto?> GetByIdAsync(int id)
        {
            var techniqueType = await _repository.GetByIdAsync(id);
            return techniqueType == null ? null : _mapper.Map<TechniqueTypeDto>(techniqueType);
        }

        public async Task<TechniqueTypeDto> CreateAsync(CreateTechniqueTypeDto dto)
        {
            // Validate name uniqueness
            if (await _repository.NameExistsAsync(dto.Name))
            {
                throw new InvalidOperationException($"TechniqueType with name '{dto.Name}' already exists.");
            }

            var techniqueType = _mapper.Map<TechniqueType>(dto);
            var created = await _repository.CreateAsync(techniqueType);
            return _mapper.Map<TechniqueTypeDto>(created);
        }

        public async Task<TechniqueTypeDto> UpdateAsync(int id, UpdateTechniqueTypeDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"TechniqueType with ID {id} not found.");
            }

            // Validate name uniqueness (excluding current record)
            if (await _repository.NameExistsAsync(dto.Name, id))
            {
                throw new InvalidOperationException($"TechniqueType with name '{dto.Name}' already exists.");
            }

            _mapper.Map(dto, existing);
            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<TechniqueTypeDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!await _repository.ExistsAsync(id))
            {
                throw new KeyNotFoundException($"TechniqueType with ID {id} not found.");
            }

            // Kiểm tra xem có technique nào đang dùng type này không
            if (await _repository.HasRelatedTechniquesAsync(id))
            {
                throw new InvalidOperationException(
                    $"Cannot delete TechniqueType with ID {id} because it is being used by one or more Techniques.");
            }

            return await _repository.DeleteAsync(id);
        }

        public async Task<PagedResult<TechniqueTypeDto>> GetAllTechniqueTypesAsync(int page, int pageSize)
        {
            var pagedTechniqueTypes = await _repository.GetAllTechniqueTypesAsync(page, pageSize);

            var techniqueTypeDtos = _mapper.Map<List<TechniqueTypeDto>>(pagedTechniqueTypes.Data);

            return new PagedResult<TechniqueTypeDto>
            {
                Data = techniqueTypeDtos,
                CurrentPage = pagedTechniqueTypes.CurrentPage,
                PageSize = pagedTechniqueTypes.PageSize,
                TotalItems = pagedTechniqueTypes.TotalItems,
                TotalPages = pagedTechniqueTypes.TotalPages
            };
        }
    }
}
