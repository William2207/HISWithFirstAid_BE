using FirstAidAPI.DTO;
using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IQuizQuestionRepository
    {
        Task<IEnumerable<QuizQuestion>> GetAllAsync();

        Task<QuizQuestion?> GetByIdAsync(int id);

        Task<IEnumerable<QuizQuestion>> GetByTechniqueIdAsync(int techniqueId);

        Task AddAsync(QuizQuestion quizQuestion);

        Task UpdateAsync(QuizQuestion quizQuestion);

        void Delete(QuizQuestion quizQuestion);

        Task<bool> ExistsAsync(int id);

        Task<bool> SaveChangesAsync();

        Task<bool> DeleteAnswerOptionAsync(int answerOptionId);

        Task<PagedResult<QuizQuestion>> GetAllQuizQuestionsAsync(int page, int pageSize);
    }
}