using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly FirstAidContext _context;

        public DoctorRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<Doctor> AddAsync(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }

        public async Task UpdateAsync(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Specialty)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Doctor?> GetByUserIdAsync(int userId)
        {
            return await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Specialty)
                .FirstOrDefaultAsync(d => d.UserId == userId);
        }

        public async Task<List<Doctor>> GetAllAsync()
        {
            return await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Specialty)
                .ToListAsync();
        }

        public async Task<bool> ExistsByUserIdAsync(int userId)
        {
            return await _context.Doctors
                .AnyAsync(d => d.UserId == userId);
        }

        public async Task<List<Doctor>> GetDoctorsBySpecialtyForBookingAsync(int specialtyId)
        {
            return await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Specialty)
                .Include(d => d.Schedules)
                    .ThenInclude(s => s.Clinic)
                .Where(d => d.IsAvailable && d.SpecialtyId == specialtyId)
                .ToListAsync();
        }

        public async Task<Clinic?> GetDefaultClinicBySpecialtyAsync(int specialtyId)
        {
            return await _context.Clinics.FirstOrDefaultAsync(c => c.SpecialtyId == specialtyId);
        }

        public async Task<DoctorSchedule?> GetScheduleAsync(int doctorId, DayOfWeek dayOfWeek, TimeSpan timeOfDay)
        {
            return await _context.DoctorSchedules
                .FirstOrDefaultAsync(s => s.DoctorId == doctorId
                                       && s.DayOfWeek == dayOfWeek
                                       && s.StartTime <= timeOfDay
                                       && s.EndTime >= timeOfDay);
        }

        public async Task UpdateScheduleAsync(DoctorSchedule schedule)
        {
            _context.DoctorSchedules.Update(schedule);
            await _context.SaveChangesAsync();
        }
    }
}
