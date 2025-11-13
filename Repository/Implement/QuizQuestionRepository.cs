using FirstAidAPI.Data;
using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;
using FirstAidAPI.Extensions;

namespace FirstAidAPI.Repository.Implement
{
    public class QuizQuestionRepository : IQuizQuestionRepository
    {
        private readonly FirstAidContext _context;

        public QuizQuestionRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QuizQuestion>> GetAllAsync()
        {
            return await _context.QuizQuestions
                .AsNoTracking()
                .Include(q => q.Technique)
                .Include(q => q.AnswerOptions)
                .OrderBy(q => q.Id)
                .ToListAsync();
        }

        public async Task<QuizQuestion?> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _context.QuizQuestions
                .Include(q => q.Technique)
                .Include(q => q.AnswerOptions)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<IEnumerable<QuizQuestion>> GetByTechniqueIdAsync(int techniqueId)
        {
            return await _context.QuizQuestions
                .AsNoTracking()
                .Include(q => q.AnswerOptions)
                .Where(q => q.TechniqueId == techniqueId)
                .OrderBy(q => q.Id)
                .ToListAsync();
        }

        public async Task AddAsync(QuizQuestion quizQuestion)
        {
            await _context.QuizQuestions.AddAsync(quizQuestion);
        }

        public Task UpdateAsync(QuizQuestion quizQuestion)
        {
            var entry = _context.Entry(quizQuestion);
            if (entry.State == EntityState.Detached)
            {
                _context.QuizQuestions.Attach(quizQuestion); // hoặc Update nếu muốn
                entry.State = EntityState.Modified; // chỉ cần với chính entity
            }
            return Task.CompletedTask;
        }

        public void Delete(QuizQuestion quizQuestion)
        {
            _context.QuizQuestions.Remove(quizQuestion);
        }

        public async Task<bool> DeleteAnswerOptionAsync(int answerOptionId)
        {
            if (answerOptionId <= 0)
                return false;

            // Tìm answer option
            var answerOption = await _context.AnswerOptions
                .Include(a => a.QuizQuestion)
                    .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync(a => a.Id == answerOptionId);

            if (answerOption == null)
                return false;

            // Validation: Không cho xóa nếu chỉ còn 2 đáp án
            var totalAnswers = answerOption.QuizQuestion.AnswerOptions.Count;
            if (totalAnswers <= 2)
            {
                throw new InvalidOperationException("Không thể xóa. Câu hỏi phải có ít nhất 2 đáp án.");
            }

            // Validation: Không cho xóa nếu đây là đáp án đúng duy nhất
            if (answerOption.IsCorrect)
            {
                var correctAnswersCount = answerOption.QuizQuestion.AnswerOptions
                    .Count(a => a.IsCorrect);

                if (correctAnswersCount <= 1)
                {
                    throw new InvalidOperationException("Không thể xóa. Phải có ít nhất 1 đáp án đúng.");
                }
            }

            _context.AnswerOptions.Remove(answerOption);
            return await SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.QuizQuestions.AnyAsync(q => q.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}