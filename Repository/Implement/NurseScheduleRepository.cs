using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class NurseScheduleRepository : INurseScheduleRepository
    {
        private readonly FirstAidContext _context;

        public NurseScheduleRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<NurseSchedule?> GetByNurseAndDateAsync(int nurseId, DateOnly date)
        {
            return await _context.NurseSchedules
                .Include(s => s.Ward)
                .FirstOrDefaultAsync(s => s.NurseId == nurseId && s.Date == date);
        }

        public async Task<List<NurseSchedule>> GetByMonthAsync(int month, int year)
        {
            return await _context.NurseSchedules
                .Include(s => s.Nurse)
                .Include(s => s.Ward)
                .Where(s => s.Date.Month == month && s.Date.Year == year)
                .ToListAsync();
        }

        public async Task<List<NurseSchedule>> GetByNurseAndMonthAsync(int nurseId, int month, int year)
        {
            return await _context.NurseSchedules
                .Include(s => s.Nurse)
                    .ThenInclude(n => n.User)
                .Include(s => s.Ward)
                .Where(s => s.NurseId == nurseId
                         && s.Date.Month == month
                         && s.Date.Year == year)
                .ToListAsync();
        }

        public async Task<List<NurseSchedule>> GetBySpecialtyAndMonthAsync(int specialtyId, int month, int year)
        {
            return await _context.NurseSchedules
                .Include(s => s.Nurse)
                    .ThenInclude(n => n.User)
                .Include(s => s.Ward)
                .Where(s => s.Nurse.SpecialityId == specialtyId
                         && s.Date.Month == month
                         && s.Date.Year == year)
                .ToListAsync();
        }

        public async Task<bool> ExistsForMonthAsync(int month, int year)
        {
            return await _context.NurseSchedules
                .AnyAsync(s => s.Date.Month == month && s.Date.Year == year);
        }

        public async Task<bool> ExistsForSpecialtyAndMonthAsync(int specialtyId, int month, int year)
        {
            return await _context.NurseSchedules
                .AnyAsync(s => s.Nurse.SpecialityId == specialtyId && s.Date.Month == month && s.Date.Year == year);
        }

        public async Task AddRangeAsync(List<NurseSchedule> schedules)
        {
            await _context.NurseSchedules.AddRangeAsync(schedules);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByMonthAsync(int month, int year)
        {
            var schedules = await _context.NurseSchedules
                .Where(s => s.Date.Month == month && s.Date.Year == year)
                .ToListAsync();

            _context.NurseSchedules.RemoveRange(schedules);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBySpecialtyAndMonthAsync(int specialtyId, int month, int year)
        {
            var schedules = await _context.NurseSchedules
                .Where(s => s.Nurse.SpecialityId == specialtyId && s.Date.Month == month && s.Date.Year == year)
                .ToListAsync();

            _context.NurseSchedules.RemoveRange(schedules);
            await _context.SaveChangesAsync();
        }
    }
}
