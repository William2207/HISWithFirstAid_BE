using FirstAidAPI.DTO;
using FirstAidAPI.DTO.Quiz;

namespace FirstAidAPI.Service
{
    public interface IQuizQuestionService
    {
        Task<IEnumerable<QuizQuestionDto>> GetAllAsync();

        Task<QuizQuestionDto?> GetByIdAsync(int id);

        Task<IEnumerable<QuizQuestionDto>> GetByTechniqueIdAsync(int techniqueId);

        Task<QuizQuestionDto> CreateAsync(CreateQuizQuestionDto createDto);

        Task<QuizQuestionDto> UpdateAsync(int id, UpdateQuizQuestionDto updateDto);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<bool> DeleteAnswerOptionAsync(int answerOptionId);

        Task<PagedResult<QuizQuestionDto>> GetAllQuizQuestionsAsync(int page, int pageSize);
    }
}