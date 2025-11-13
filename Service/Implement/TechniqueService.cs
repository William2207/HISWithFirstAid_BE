using AutoMapper;
using FirstAidAPI.DTO;
using FirstAidAPI.Extensions;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstAidAPI.Service.Implement
{
    public class TechniqueService : ITechniqueService
    {
        private readonly ITechniqueRepository _techniqueRepository;
        private readonly ITechniqueStepRepository _techniqueStepRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TechniqueService> _logger;

        // Inject ITechniqueRepository vào constructor
        public TechniqueService(ITechniqueRepository techniqueRepository, IMapper mapper, ITechniqueStepRepository techniqueStepRepository,
        ILogger<TechniqueService> logger)
        {
            _techniqueRepository = techniqueRepository;
            _techniqueStepRepository = techniqueStepRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<Technique>> GetAllTechniquesAsync()
        {
            return await _techniqueRepository.GetAllAsync();
        }

        public async Task<PagedResult<Technique>> GetTechniquesAsync(int page, int pageSize, List<string>? difficulties, List<int>? typeIds, string? search)
        {
            // Business logic/validation được chuyển về đây
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 9;
            if (pageSize > 100) pageSize = 100; // Giới hạn page size là một business rule

            return await _techniqueRepository.GetAllFilteredAndPagedAsync(page, pageSize, difficulties, typeIds, search);
        }

        public async Task<Technique?> GetTechniqueByIdAsync(int id)
        {
            return await _techniqueRepository.GetByIdAsync(id);
        }

        public async Task<TechniqueDto> CreateAsync(CreateTechniqueDto dto)
        {
            // Validate TechniqueType exists
            var techniqueTypeExists = await _techniqueRepository.TechniqueTypeExistsAsync(dto.TechniqueTypeId);
            if (!techniqueTypeExists)
            {
                _logger.LogWarning("Attempted to create technique with non-existent TechniqueTypeId: {TechniqueTypeId}", dto.TechniqueTypeId);
                throw new NotFoundException($"TechniqueType with ID {dto.TechniqueTypeId} not found");
            }

            ValidateSteps(dto.Steps);
            using var transaction = await _techniqueRepository.BeginTransactionAsync();

            try
            {
                // Tạo Technique
                var technique = new Technique
                {
                    Name = dto.Name,
                    Title = dto.Title,
                    Description = dto.Description,
                    Difficulty = dto.Difficulty,
                    VideoUrl = dto.VideoUrl,
                    ImageUrl = dto.ImageUrl,
                    Duration = dto.Duration,
                    Icon = dto.Icon,
                    TechniqueTypeId = dto.TechniqueTypeId
                };

                technique = await _techniqueRepository.AddAsync(technique);

                // Tạo Steps
                var createdSteps = new List<TechniqueStep>();
                if (dto.Steps != null && dto.Steps.Any())
                {
                    foreach (var stepDto in dto.Steps)
                    {
                        var step = new TechniqueStep
                        {
                            TechniqueId = technique.Id,
                            StepNumber = stepDto.StepNumber,
                            Instruction = stepDto.Instruction,
                            ExpectedAction = stepDto.ExpectedAction,
                            Duration = stepDto.Duration,
                            ImageUrl = stepDto.ImageUrl
                        };

                        var createdStep = await _techniqueStepRepository.AddAsync(step);
                        createdSteps.Add(createdStep);
                    }
                }

                // ✨ Commit transaction
                await _techniqueRepository.CommitTransactionAsync();

                _logger.LogInformation("Created technique {TechniqueId} with {StepCount} steps",
                    technique.Id, createdSteps.Count);

                // Trả về DTO
                return new TechniqueDto
                {
                    Id = technique.Id,
                    Name = technique.Name,
                    Title = technique.Title,
                    Description = technique.Description,
                    Difficulty = technique.Difficulty,
                    VideoUrl = technique.VideoUrl,
                    ImageUrl = technique.ImageUrl,
                    Duration = technique.Duration,
                    Icon = technique.Icon,
                    TechniqueTypeId = technique.TechniqueTypeId,
                    TechniqueTypeName = technique.Type?.Name
                };
            }
            catch (Exception ex)
            {
                // ✨ Rollback nếu có lỗi
                await _techniqueRepository.RollbackTransactionAsync();
                _logger.LogError(ex, "Failed to create technique with steps");
                throw;
            }
        }

        public async Task<TechniqueDto?> UpdateAsync(int id, UpdateTechniqueDto dto)
        {
            var technique = await _techniqueRepository.GetByIdAsync(id);

            if (technique == null)
            {
                _logger.LogWarning("Attempted to update non-existent technique with ID: {TechniqueId}", id);
                return null;
            }

            // Validate TechniqueType if being updated
            if (dto.TechniqueTypeId.HasValue)
            {
                var techniqueTypeExists = await _techniqueRepository.TechniqueTypeExistsAsync(dto.TechniqueTypeId.Value);
                if (!techniqueTypeExists)
                {
                    _logger.LogWarning("Attempted to update technique with non-existent TechniqueTypeId: {TechniqueTypeId}", dto.TechniqueTypeId.Value);
                    throw new NotFoundException($"TechniqueType with ID {dto.TechniqueTypeId.Value} not found");
                }

                technique.TechniqueTypeId = dto.TechniqueTypeId.Value;
            }

            // Update only provided fields
            if (dto.Name != null) technique.Name = dto.Name;
            if (dto.Title != null) technique.Title = dto.Title;
            if (dto.Description != null) technique.Description = dto.Description;
            if (dto.Difficulty != null) technique.Difficulty = dto.Difficulty;
            if (dto.VideoUrl != null) technique.VideoUrl = dto.VideoUrl;
            if (dto.ImageUrl != null) technique.ImageUrl = dto.ImageUrl;
            if (dto.Duration.HasValue) technique.Duration = dto.Duration.Value;
            if (dto.Icon != null) technique.Icon = dto.Icon;

            technique = await _techniqueRepository.UpdateAsync(technique);

            _logger.LogInformation("Updated technique with ID: {TechniqueId}", id);

            return new TechniqueDto
            {
                Id = technique.Id,
                Name = technique.Name,
                Title = technique.Title,
                Description = technique.Description,
                Difficulty = technique.Difficulty,
                VideoUrl = technique.VideoUrl,
                ImageUrl = technique.ImageUrl,
                Duration = technique.Duration,
                Icon = technique.Icon,
                TechniqueTypeId = technique.TechniqueTypeId,
                TechniqueTypeName = technique.Type?.Name
            };
        }

        //Update Step info
        public async Task<List<TechniqueStepDto>> UpdateStepsAsync(int techniqueId, UpdateTechniqueStepsDto dto)
        {
            // Validate technique exists
            var technique = await _techniqueRepository.GetByIdAsync(techniqueId);
            if (technique == null)
            {
                throw new NotFoundException($"Technique with ID {techniqueId} not found");
            }

            var updatedSteps = new List<TechniqueStep>();

            using var transaction = await _techniqueStepRepository.BeginTransactionAsync();
            try
            {
                foreach (var stepDto in dto.Steps)
                {
                    var step = await _techniqueStepRepository.GetByIdAsync(stepDto.Id);

                    if (step == null || step.TechniqueId != techniqueId)
                    {
                        throw new NotFoundException($"Step with ID {stepDto.Id} not found for technique {techniqueId}");
                    }

                    // Update only provided fields
                    if (stepDto.StepNumber.HasValue) step.StepNumber = stepDto.StepNumber.Value;
                    if (stepDto.Instruction != null) step.Instruction = stepDto.Instruction;
                    if (stepDto.ExpectedAction != null) step.ExpectedAction = stepDto.ExpectedAction;
                    if (stepDto.Duration.HasValue) step.Duration = stepDto.Duration.Value;
                    if (stepDto.ImageUrl != null) step.ImageUrl = stepDto.ImageUrl;

                    var updated = await _techniqueStepRepository.UpdateAsync(step);
                    updatedSteps.Add(updated);
                }

                await transaction.CommitAsync();

                _logger.LogInformation("Updated {StepCount} steps for technique {TechniqueId}",
                    updatedSteps.Count, techniqueId);

                return updatedSteps.Select(s => new TechniqueStepDto
                {
                    Id = s.Id,
                    StepNumber = s.StepNumber,
                    Instruction = s.Instruction,
                    ExpectedAction = s.ExpectedAction,
                    Duration = s.Duration,
                    ImageUrl = s.ImageUrl
                }).ToList();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deleted = await _techniqueRepository.DeleteAsync(id);

            if (deleted)
            {
                _logger.LogInformation("Deleted technique with ID: {TechniqueId}", id);
            }
            else
            {
                _logger.LogWarning("Attempted to delete non-existent technique with ID: {TechniqueId}", id);
            }

            return deleted;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _techniqueRepository.ExistsAsync(id);
        }

        private void ValidateSteps(List<CreateTechniqueStepDto>? steps)
        {
            if (steps == null || !steps.Any()) return;

            var duplicateStepNumbers = steps
                .GroupBy(s => s.StepNumber)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateStepNumbers.Any())
            {
                throw new BadRequestException($"Duplicate step numbers: {string.Join(", ", duplicateStepNumbers)}");
            }

            var orderedSteps = steps.OrderBy(s => s.StepNumber).ToList();
            for (int i = 0; i < orderedSteps.Count; i++)
            {
                if (orderedSteps[i].StepNumber != i + 1)
                {
                    throw new BadRequestException("Step numbers must be sequential starting from 1");
                }
            }
        }
    }
}