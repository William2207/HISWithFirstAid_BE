using FirstAidAPI.Data;
using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;
using FirstAidAPI.Extensions;
using Npgsql;

namespace FirstAidAPI.Repository.Implement
{
    public class PracticalCourseRepository : IPracticalCourseRepository
    {
        private readonly FirstAidContext _context;

        public PracticalCourseRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PracticalCourse>> GetAllAsync()
        {
            return await _context.PracticalCourses.ToListAsync();
        }

        public async Task<PracticalCourse?> GetByIdAsync(int id)
        {
            return await _context.PracticalCourses.FindAsync(id);
        }

        public async Task<PracticalCourse> UpdateAsync(PracticalCourse course)
        {
            _context.PracticalCourses.Update(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<List<PracticalCourse>> GetByIdsWithLockAsync(List<int> courseIds)
        {
            if (!courseIds.Any())
                return new List<PracticalCourse>();

            var parameters = courseIds.Select((id, index) =>
                new NpgsqlParameter($"@p{index}", id)
            ).ToArray();

            var parameterNames = string.Join(",",
                courseIds.Select((_, index) => $"@p{index}")
            );

            var sql = $@"
            SELECT * FROM ""PracticalCourses""
            WHERE ""Id"" IN ({parameterNames})
            FOR UPDATE";

            return await _context.PracticalCourses
                .FromSqlRaw(sql, parameters)
                .ToListAsync();
        }

        public async Task<List<PracticalCourse>> GetByIdsAsync(List<int> courseIds)
        {
            return await _context.PracticalCourses
                .Where(c => courseIds.Contains(c.Id))
                .ToListAsync();
        }
    }
}