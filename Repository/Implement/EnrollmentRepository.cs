using FirstAidAPI.Data;
using FirstAidAPI.DTO;
using FirstAidAPI.Extensions;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly FirstAidContext _context;

        public EnrollmentRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<CourseEnrollment?> GetByIdAsync(int id)
        {
            return await _context.CourseEnrollments
                .Include(e => e.User)
                .Include(e => e.PracticalCourse)
                .Include(e => e.Order)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<CourseEnrollment?> GetByUserAndCourseAsync(int userId, int courseId)
        {
            return await _context.CourseEnrollments
                .FirstOrDefaultAsync(e =>
                    e.UserId == userId &&
                    e.PracticalCourseId == courseId);
        }

        public async Task<List<CourseEnrollment>> GetByUserIdAsync(int userId)
        {
            return await _context.CourseEnrollments
                .Include(e => e.PracticalCourse)
                .Include(e => e.Order)
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.EnrolledAt)
                .ToListAsync();
        }

        public async Task<List<CourseEnrollment>> GetByCourseIdAsync(int courseId)
        {
            return await _context.CourseEnrollments
                .Include(e => e.User)
                .Include(e => e.Order)
                .Where(e => e.PracticalCourseId == courseId)
                .OrderBy(e => e.EnrolledAt)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int userId, int courseId)
        {
            return await _context.CourseEnrollments
                .AnyAsync(e => e.UserId == userId && e.PracticalCourseId == courseId);
        }

        public async Task<CourseEnrollment> AddAsync(CourseEnrollment enrollment)
        {
            await _context.CourseEnrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task UpdateAsync(CourseEnrollment enrollment)
        {
            _context.CourseEnrollments.Update(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var enrollment = await _context.CourseEnrollments.FindAsync(id);
            if (enrollment != null)
            {
                _context.CourseEnrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CountByCourseIdAsync(int courseId)
        {
            return await _context.CourseEnrollments
                .CountAsync(e => e.PracticalCourseId == courseId);
        }

        public async Task<PagedResult<CourseEnrollment>> GetUserEnrollmentsAsync(int userId, int page, int pageSize)
        {
            var query = _context.CourseEnrollments
                .Where(e => e.UserId == userId)
                .Include(e => e.PracticalCourse)
                .OrderByDescending(e => e.EnrolledAt);

            return await query.ToPagedResultAsync(page, pageSize);
        }
    }
}