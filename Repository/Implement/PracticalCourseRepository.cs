using FirstAidAPI.Data;
using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;
using FirstAidAPI.Extensions;
using Npgsql;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public async Task<PracticalCourse> CreateAsync(PracticalCourse course)
        {
            _context.PracticalCourses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _context.PracticalCourses.FindAsync(id);
            if (course != null)
            {
                _context.PracticalCourses.Remove(course);
                await _context.SaveChangesAsync();
            }
            else
            {
                return false;
            }
            return true;
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

        public async Task<PagedResult<PracticalCourse>> GetPagedAsync(int page, int pageSize, string? search)
        {
            var query = _context.PracticalCourses.AsQueryable();

            // Nếu có search thì filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(c =>
                    c.Title.ToLower().Contains(searchLower) ||
                    c.Description.ToLower().Contains(searchLower) ||
                    c.Location.ToLower().Contains(searchLower));
            }

            // Sắp xếp
            query = query.OrderByDescending(c => c.Id);

            return await query.ToPagedResultAsync(page, pageSize);
        }
    }
}
