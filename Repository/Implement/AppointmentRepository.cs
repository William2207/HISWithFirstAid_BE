using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FirstAidAPI.Repository.Implement
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly FirstAidContext _context;

        public AppointmentRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<Appointment> AddAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment> UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment> DeleteAsync(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.User)
                .Include(a => a.Specialty)
                .Include(a => a.MedicalRecord)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Appointment>> GetWaitingAppointmentsByDoctorAsync(int doctorId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.User)
                .Include(a => a.Specialty)
                .Include(a => a.MedicalRecord)
                .Where(a => a.DoctorId == doctorId &&
                            (a.Status == FirstAidAPI.Enums.AppointmentStatus.Registered ||
                             a.Status == FirstAidAPI.Enums.AppointmentStatus.In_Progress))
                .OrderBy(a => a.AppointmentDateTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAndDateAsync(int doctorId, DateTime date)
        {
            var startUtc = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
            var endUtc = startUtc.AddDays(1);

            return await _context.Appointments
                .Include(a => a.Patient).ThenInclude(p => p.User)
                .Include(a => a.Doctor).ThenInclude(d => d.User)
                .Include(a => a.Specialty)
                .Include(a => a.MedicalRecord)
                .Where(a => a.DoctorId == doctorId
                    && a.AppointmentDateTime >= startUtc
                    && a.AppointmentDateTime < endUtc)
                .OrderBy(a => a.AppointmentDateTime)
                .ThenBy(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.User)
                .Include(a => a.Specialty)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDateTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetCompletedAppointmentsAsync()
        {
            // Only get completed appointments that do not have an Invoice generated yet
            return await _context.Appointments
                .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.User)
                .Include(a => a.Specialty)
                .Where(a => a.Status == FirstAidAPI.Enums.AppointmentStatus.Completed &&
                            !_context.Invoices.Any(i => i.AppointmentId == a.Id))
                .OrderByDescending(a => a.AppointmentDateTime)
                .ToListAsync();
        }

        public async Task<bool> ExistsOverlapAsync(int patientId, int specialtyId, DateTime dateTime)
        {
            return await _context.Appointments.AnyAsync(a =>
                a.PatientId == patientId &&
                a.SpecialtyId == specialtyId &&
                a.AppointmentDateTime == dateTime);
        }

        /// <inheritdoc/>
        public async Task<int> GetSlotCountWithLockAsync(int doctorId, DateTime appointmentDateTime, int appointmentType)
        {
            // Dùng raw SQL với FOR UPDATE để lock các row phù hợp,
            // ngăn transaction khác đọc/ghi cùng lúc (Pessimistic Locking).
            var sql = @"
                SELECT * FROM ""Appointments""
                WHERE ""DoctorId"" = @doctorId
                  AND ""AppointmentDateTime"" = @dateTime
                  AND ""Type"" = @type
                  AND ""Status"" != @cancelledStatus
                FOR UPDATE";

            var count = await _context.Appointments
                .FromSqlRaw(sql,
                    new NpgsqlParameter("@doctorId", doctorId),
                    new NpgsqlParameter("@dateTime", appointmentDateTime),
                    new NpgsqlParameter("@type", appointmentType),
                    new NpgsqlParameter("@cancelledStatus", (int)FirstAidAPI.Enums.AppointmentStatus.Cancelled))
                .CountAsync();

            return count;
        }
    }
}
