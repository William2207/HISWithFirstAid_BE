using AutoMapper;
using FirstAidAPI.DTO;
using FirstAidAPI.DTO.Scenario;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;

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

        public async Task<PagedResult<ScenarioDto>> GetScenariosAsync(int page, int pageSize, List<string>? difficulties, List<string>? types, string? search)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 9;
            if (pageSize > 100) pageSize = 100;

            return await _scenarioRepository.GetAllFilteredAndPagedAsync(page, pageSize, difficulties, types, search);
        }

        public async Task<ScenarioDetailDto?> GetScenarioByIdAsync(int id)
        {
            var scenario = await _scenarioRepository.GetByIdAsync(id);
            if (scenario == null)
                return null;
            return _mapper.Map<ScenarioDetailDto>(scenario);
        }

        public async Task<ScenarioDetailDto> CreateScenarioAsync(CreateScenarioDto createDto)
        {
            var scenario = _mapper.Map<Scenario>(createDto);

            // Set Order cho ScenarioSteps
            for (int i = 0; i < scenario.ScenarioSteps.Count; i++)
            {
                scenario.ScenarioSteps[i].Order = i + 1;
                //scenario.ScenarioSteps[i].ScenarioId = scenario.Id;
            }

            var createdScenario = await _scenarioRepository.CreateAsync(scenario);
            return _mapper.Map<ScenarioDetailDto>(createdScenario);
        }

        public async Task<ScenarioDetailDto> UpdateScenarioAsync(int id, UpdateScenarioDto updateDto)
        {
            var existingScenario = await _scenarioRepository.GetByIdAsync(id);

            if (existingScenario == null)
                throw new KeyNotFoundException($"Scenario with ID {id} not found");

            _mapper.Map(updateDto, existingScenario);
            if (updateDto.ScenarioSteps != null)
            {
                var incomingStepIds = new HashSet<int>(updateDto.ScenarioSteps.Where(s => s.Id > 0).Select(s => s.Id));
                var incomingOptionIds = updateDto.ScenarioSteps
                    .Where(s => s.Options != null)
                    .SelectMany(s => s.Options!)
                    .Where(o => o.Id.HasValue && o.Id > 0)
                    .Select(o => o.Id!.Value)
                    .ToHashSet();

                var stepToRemove = existingScenario.ScenarioSteps.Where(s => !incomingStepIds.Contains(s.Id)).ToList();
                foreach (var step in stepToRemove)
                {
                    existingScenario.ScenarioSteps.Remove(step);
                }

                foreach (var step in existingScenario.ScenarioSteps)
                {
                    var optionsToRemove = step.Options
                        .Where(o => !incomingOptionIds.Contains(o.Id))
                        .ToList();
                    foreach (var option in optionsToRemove)
                    {
                        step.Options.Remove(option);
                    }
                }
                var existingStepsDict = existingScenario.ScenarioSteps.ToDictionary(s => s.Id);

                foreach (var stepDto in updateDto.ScenarioSteps)
                {
                    if (stepDto.Id > 0 && existingStepsDict.TryGetValue(stepDto.Id, out var existingStep))
                    {
                        _mapper.Map(stepDto, existingStep);

                        if (stepDto.Options != null)
                        {
                            var existingOptionsDict = existingStep.Options.ToDictionary(o => o.Id);

                            foreach (var optionDto in stepDto.Options)
                            {
                                if (optionDto.Id.HasValue && optionDto.Id is > 0)
                                {
                                    if (existingOptionsDict.TryGetValue(optionDto.Id.Value, out var existingOption))
                                    {
                                        _mapper.Map(optionDto, existingOption);
                                    }
                                    else
                                    {
                                        throw new KeyNotFoundException($"Option with ID {optionDto.Id} not found in Step ID {existingStep.Id}");
                                    }
                                }
                                else
                                {
                                    var newOption = _mapper.Map<StepOption>(optionDto);
                                    newOption.StepId = existingStep.Id;
                                    existingStep.Options.Add(newOption);
                                }
                            }
                        }
                    }
                    else if (stepDto.Id <= 0)
                    {
                        var newStep = _mapper.Map<ScenarioStep>(stepDto);
                        newStep.ScenarioId = existingScenario.Id;
                        existingScenario.ScenarioSteps.Add(newStep);
                    }
                }
            }
            await _scenarioRepository.UpdateAsync(existingScenario);

            return _mapper.Map<ScenarioDetailDto>(existingScenario);
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