using FirstAidAPI.Repository;
using FirstAidAPI.DTO.SavedTechniques;
using AutoMapper;
using FirstAidAPI.Models;

namespace FirstAidAPI.Service.Implement
{
    public class SavedTechniqueService : ISavedTechniqueService
    {
        private readonly ISavedTechniqueRepository _savedTechniqueRepository;
        private ILogger<SavedTechniqueService> _logger;
        private IMapper _mapper;

        public SavedTechniqueService(ISavedTechniqueRepository savedTechniqueRepository, ILogger<SavedTechniqueService> logger, IMapper mapper)
        {
            _savedTechniqueRepository = savedTechniqueRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<SavedTechniqueNewDto> AddSavedTechniqueAsync(AddSavedTechniqueDto createDto, int userId)
        {
            var savedTechnique = new SavedTechnique
            {
                UserId = userId,
                TechniqueId = createDto.TechniqueId,
                SavedAt = DateTime.UtcNow
            };
            var result = await _savedTechniqueRepository.AddSavedTechniqueAsync(savedTechnique);
            return _mapper.Map<SavedTechniqueNewDto>(result);
        }

        public async Task<IEnumerable<SavedTechniqueNewDto>> GetSavedTechniquesByUserAsync(int userId)
        {
            var savedTechniques = await _savedTechniqueRepository.GetSavedTechniquesByUserAsync(userId);
            return _mapper.Map<IEnumerable<SavedTechniqueNewDto>>(savedTechniques);
        }

        public async Task<bool> RemoveSavedTechniqueAsync(int userId, int techniqueId)
        {
            return await _savedTechniqueRepository.RemoveSavedTechniqueAsync(userId, techniqueId);
        }
    }
}