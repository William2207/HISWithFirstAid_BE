using AutoMapper;
using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstAidAPI.Service.Implement
{
    public class ScenarioService : IScenarioService
    {
        private readonly IScenarioRepository _scenarioRepository;
        private readonly IMapper _mapper;

        public ScenarioService(IScenarioRepository scenarioRepository, IMapper mapper)
        {
            _scenarioRepository = scenarioRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ScenarioDto>> GetAllScenariosAsync()
        {
            var scenarios = await _scenarioRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ScenarioDto>>(scenarios);
        }

        public async Task<PagedResult<Scenario>> GetScenariosAsync(int page, int pageSize, List<string>? difficulties, List<string>? types, string? search)
        {
            // Chuyển logic validation từ Controller vào đây
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 9;
            if (pageSize > 100) pageSize = 100;

            return await _scenarioRepository.GetAllFilteredAndPagedAsync(page, pageSize, difficulties, types, search);
        }

        public async Task<Scenario?> GetScenarioByIdAsync(int id)
        {
            return await _scenarioRepository.GetByIdAsync(id);
        }

        public async Task<ScenarioDetailDto> CreateScenarioAsync(CreateScenarioDto createDto)
        {
            var scenario = _mapper.Map<Scenario>(createDto);

            // Set ScenarioId cho ScenarioTechniques
            foreach (var technique in scenario.ScenarioTechniques)
            {
                technique.ScenarioId = scenario.Id;
            }

            // Set Order cho ScenarioSteps
            for (int i = 0; i < scenario.ScenarioSteps.Count; i++)
            {
                scenario.ScenarioSteps[i].Order = i + 1;
                scenario.ScenarioSteps[i].ScenarioId = scenario.Id;
            }

            var createdScenario = await _scenarioRepository.CreateAsync(scenario);
            return _mapper.Map<ScenarioDetailDto>(createdScenario);
        }

        public async Task<ScenarioDetailDto> UpdateScenarioAsync(int id, UpdateScenarioDto updateDto)
        {
            var existingScenario = await _scenarioRepository.GetByIdAsync(id);
            if (existingScenario == null)
                throw new KeyNotFoundException($"Scenario with ID {id} not found");

            // Xóa các ScenarioTechniques cũ
            existingScenario.ScenarioTechniques.Clear();

            // Xóa các ScenarioSteps cũ
            existingScenario.ScenarioSteps.Clear();

            // Map dữ liệu mới
            _mapper.Map(updateDto, existingScenario);

            // Set ScenarioId cho ScenarioTechniques
            foreach (var technique in existingScenario.ScenarioTechniques)
            {
                technique.ScenarioId = id;
            }

            // Set Order và ScenarioId cho ScenarioSteps
            for (int i = 0; i < existingScenario.ScenarioSteps.Count; i++)
            {
                existingScenario.ScenarioSteps[i].Order = i + 1;
                existingScenario.ScenarioSteps[i].ScenarioId = id;
            }

            var updatedScenario = await _scenarioRepository.UpdateAsync(existingScenario);
            return _mapper.Map<ScenarioDetailDto>(updatedScenario);
        }

        public async Task<bool> DeleteScenarioAsync(int id)
        {
            var exists = await _scenarioRepository.ExistsAsync(id);
            if (!exists)
                throw new KeyNotFoundException($"Scenario with ID {id} not found");

            return await _scenarioRepository.DeleteAsync(id);
        }
    }
}