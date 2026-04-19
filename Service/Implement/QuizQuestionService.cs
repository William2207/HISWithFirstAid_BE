using AutoMapper;
using FirstAidAPI.DTO;
using FirstAidAPI.DTO.Quiz;
using FirstAidAPI.Extensions;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;

namespace FirstAidAPI.Service.Implement
{
    public class QuizQuestionService : IQuizQuestionService
    {
        private readonly IQuizQuestionRepository _repository;
        private readonly ITechniqueRepository _techniqueRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<QuizQuestionService> _logger;

        public QuizQuestionService(
            IQuizQuestionRepository repository,
            ITechniqueRepository techniqueRepository,
            IMapper mapper,
            ILogger<QuizQuestionService> logger)
        {
            _repository = repository;
            _techniqueRepository = techniqueRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<QuizQuestionDto>> GetAllAsync()
        {
            var questions = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<QuizQuestionDto>>(questions);
        }

        public async Task<QuizQuestionDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            var question = await _repository.GetByIdAsync(id);
            if (question == null)
                return null;

            return _mapper.Map<QuizQuestionDto>(question);
        }

        public async Task<IEnumerable<QuizQuestionDto>> GetByTechniqueIdAsync(int techniqueId)
        {
            if (techniqueId <= 0)
                return Enumerable.Empty<QuizQuestionDto>();

            var questions = await _repository.GetByTechniqueIdAsync(techniqueId);
            return _mapper.Map<IEnumerable<QuizQuestionDto>>(questions);
        }

        public async Task<PagedResult<QuizQuestionDto>> GetAllQuizQuestionsAsync(int page, int pageSize)
        {
            var pagedQuestions = await _repository.GetAllQuizQuestionsAsync(page, pageSize);
            return MapToPagedDto(pagedQuestions);
        }

        public async Task<QuizQuestionDto> CreateAsync(CreateQuizQuestionDto createDto)
        {
            // Validate Technique exists
            var techniqueExists = await _techniqueRepository.GetByIdAsync(createDto.TechniqueId);
            if (techniqueExists == null)
            {
                throw new ArgumentException($"Technique with ID {createDto.TechniqueId} not found");
            }

            // Validate at least one correct answer if AnswerOptions provided
            if (createDto.AnswerOptions != null && createDto.AnswerOptions.Any())
            {
                if (!createDto.AnswerOptions.Any(a => a.IsCorrect))
                {
                    throw new ArgumentException("At least one answer option must be marked as correct");
                }
            }

            var question = _mapper.Map<QuizQuestion>(createDto);

            await _repository.AddAsync(question);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Created quiz question with ID {Id}", question.Id);

            return _mapper.Map<QuizQuestionDto>(question);
        }

        public async Task<QuizQuestionDto> UpdateAsync(int id, UpdateQuizQuestionDto updateDto)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID", nameof(id));
            }

            var existingQuestion = await _repository.GetByIdAsync(id);
            if (existingQuestion == null)
            {
                throw new KeyNotFoundException($"Quiz question with ID {id} not found");
            }

            // Validate Technique exists if changed
            if (existingQuestion.TechniqueId != updateDto.TechniqueId)
            {
                var techniqueExists = await _techniqueRepository.GetByIdAsync(updateDto.TechniqueId);
                if (techniqueExists == null)
                {
                    throw new ArgumentException($"Technique with ID {updateDto.TechniqueId} not found");
                }
            }

            // Validate AnswerOptions (nếu có)
            if (updateDto.AnswerOptions != null && updateDto.AnswerOptions.Any())
            {
                // Kiểm tra có ít nhất 2 đáp án
                if (updateDto.AnswerOptions.Count < 2)
                {
                    throw new ArgumentException("Câu hỏi phải có ít nhất 2 đáp án");
                }

                // Kiểm tra có ít nhất 1 đáp án đúng
                if (!updateDto.AnswerOptions.Any(a => a.IsCorrect))
                {
                    throw new ArgumentException("Phải có ít nhất 1 đáp án đúng");
                }

                // Kiểm tra không có quá nhiều đáp án đúng (nếu chỉ cho phép 1 đáp án đúng)
                var correctAnswersCount = updateDto.AnswerOptions.Count(a => a.IsCorrect);
                if (correctAnswersCount > 1)
                {
                    throw new ArgumentException("Chỉ được có 1 đáp án đúng");
                }
            }

            // Map basic properties
            existingQuestion.TechniqueId = updateDto.TechniqueId;
            existingQuestion.QuestionText = updateDto.QuestionText;
            existingQuestion.Difficulty = updateDto.Difficulty;

            if (updateDto.AnswerOptions != null)
            {
                // Lấy danh sách IDs của options mới
                var newOptionIds = updateDto.AnswerOptions
                    .Where(a => a.Id.HasValue && a.Id.Value > 0)
                    .Select(a => a.Id!.Value)
                    .ToList();

                // Xóa các options không còn trong danh sách mới
                var optionsToRemove = existingQuestion.AnswerOptions
                    .Where(a => !newOptionIds.Contains(a.Id))
                    .ToList();

                foreach (var option in optionsToRemove)
                {
                    existingQuestion.AnswerOptions.Remove(option);
                }

                // Update hoặc thêm mới các options
                foreach (var dtoOption in updateDto.AnswerOptions)
                {
                    _logger.LogInformation("Processing option: Id={Id}, Text={Text}, IsCorrect={IsCorrect}", dtoOption.Id, dtoOption.AnswerText, dtoOption.IsCorrect);
                    if (dtoOption.Id.HasValue && dtoOption.Id.Value > 0)
                    {
                        _logger.LogInformation("Updating existing option {Id}", dtoOption.Id);
                        // Update existing option
                        var existingOption = existingQuestion.AnswerOptions
                            .FirstOrDefault(a => a.Id == dtoOption.Id.Value);

                        if (existingOption != null)
                        {
                            existingOption.AnswerText = dtoOption.AnswerText;
                            existingOption.IsCorrect = dtoOption.IsCorrect;
                        }
                    }
                    else
                    {
                        _logger.LogInformation("Adding new option: {Text}", dtoOption.AnswerText);
                        // Add new option
                        existingQuestion.AnswerOptions.Add(new AnswerOption
                        {
                            AnswerText = dtoOption.AnswerText,
                            IsCorrect = dtoOption.IsCorrect,
                            QuizQuestionId = existingQuestion.Id
                        });
                    }
                }
            }

            await _repository.UpdateAsync(existingQuestion);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Updated quiz question with ID {Id} and {AnswerCount} answer options",
                id, updateDto.AnswerOptions?.Count ?? 0);

            return _mapper.Map<QuizQuestionDto>(existingQuestion);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID", nameof(id));
            }

            var question = await _repository.GetByIdAsync(id);
            if (question == null)
            {
                throw new KeyNotFoundException($"Quiz question with ID {id} not found");
            }

            _repository.Delete(question);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Deleted quiz question with ID {Id}", id);
        }

        public async Task<bool> DeleteAnswerOptionAsync(int answerOptionId)
        {
            if (answerOptionId <= 0)
            {
                throw new ArgumentException("Invalid answer option ID", nameof(answerOptionId));
            }
            var deleted = await _repository.DeleteAnswerOptionAsync(answerOptionId);
            if (deleted)
            {
                await _repository.SaveChangesAsync();
                _logger.LogInformation("Deleted answer option with ID {Id}", answerOptionId);
            }
            else
            {
                _logger.LogWarning("Attempted to delete non-existent answer option with ID {Id}", answerOptionId);
            }
            return deleted;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        private PagedResult<QuizQuestionDto> MapToPagedDto(PagedResult<QuizQuestion> pagedQuestions)
        {
            var questionDtos = _mapper.Map<List<QuizQuestionDto>>(pagedQuestions.Data);

            return new PagedResult<QuizQuestionDto>
            {
                Data = questionDtos,
                CurrentPage = pagedQuestions.CurrentPage,
                PageSize = pagedQuestions.PageSize,
                TotalItems = pagedQuestions.TotalItems,
                TotalPages = pagedQuestions.TotalPages
            };
        }
    }
}
