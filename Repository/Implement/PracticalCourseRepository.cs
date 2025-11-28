using FirstAidAPI.Data;
using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;
using FirstAidAPI.Extensions;

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
    }
}