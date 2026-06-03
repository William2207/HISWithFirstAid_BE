using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class DoctorScheduleRepository : IDoctorScheduleRepository
    {
        private readonly FirstAidContext _context;

        public DoctorScheduleRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<DoctorSchedule?> GetByDoctorAndDateAsync(int doctorId, DateOnly date)
        {
            return await _context.DoctorSchedules
                .Include(s => s.Clinic)
                .Include(s => s.Ward)
                .FirstOrDefaultAsync(s => s.DoctorId == doctorId && s.Date == date);
        }

        public async Task<List<DoctorSchedule>> GetByMonthAsync(int month, int year)
        {
            return await _context.DoctorSchedules
                .Include(s => s.Doctor)
                .Include(s => s.Clinic)
                .Include(s => s.Ward)
                .Where(s => s.Date.Month == month && s.Date.Year == year)
                .ToListAsync();
        }

        public async Task<List<DoctorSchedule>> GetByDoctorAndMonthAsync(int doctorId, int month, int year)
        {
            return await _context.DoctorSchedules
                .Include(s => s.Doctor)
                    .ThenInclude(d => d.User)
                .Include(s => s.Clinic)
                .Include(s => s.Ward)
                .Where(s => s.DoctorId == doctorId
                         && s.Date.Month == month
                         && s.Date.Year == year)
                .ToListAsync();
        }

        public async Task<List<DoctorSchedule>> GetBySpecialtyAndMonthAsync(int specialtyId, int month, int year)
        {
            return await _context.DoctorSchedules
                .Include(s => s.Doctor)
                    .ThenInclude(d => d.User)
                .Include(s => s.Clinic)
                .Include(s => s.Ward)
                .Where(s => s.Doctor.SpecialtyId == specialtyId
                         && s.Date.Month == month
                         && s.Date.Year == year)
                .ToListAsync();
        }

        public async Task<bool> ExistsForMonthAsync(int month, int year)
        {
            return await _context.DoctorSchedules
                .AnyAsync(s => s.Date.Month == month && s.Date.Year == year);
        }

        public async Task<bool> ExistsForSpecialtyAndMonthAsync(int specialtyId, int month, int year)
        {
            return await _context.DoctorSchedules
                .AnyAsync(s => s.Doctor.SpecialtyId == specialtyId && s.Date.Month == month && s.Date.Year == year);
        }

        public async Task AddRangeAsync(List<DoctorSchedule> schedules)
        {
            await _context.DoctorSchedules.AddRangeAsync(schedules);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByMonthAsync(int month, int year)
        {
            var schedules = await _context.DoctorSchedules
                .Where(s => s.Date.Month == month && s.Date.Year == year)
                .ToListAsync();

            _context.DoctorSchedules.RemoveRange(schedules);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBySpecialtyAndMonthAsync(int specialtyId, int month, int year)
        {
            var schedules = await _context.DoctorSchedules
                .Where(s => s.Doctor.SpecialtyId == specialtyId && s.Date.Month == month && s.Date.Year == year)
                .ToListAsync();

            _context.DoctorSchedules.RemoveRange(schedules);
            await _context.SaveChangesAsync();
        }
    }
}
