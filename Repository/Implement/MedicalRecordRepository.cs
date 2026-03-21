using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly FirstAidContext _context;

        public MedicalRecordRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<MedicalRecord> CreateAsync(MedicalRecord medicalRecord)
        {
            _context.MedicalRecords.Add(medicalRecord);
            await _context.SaveChangesAsync();
            return medicalRecord;
        }

        public async Task<MedicalRecord?> GetByIdAsync(int id)
        {
            return await _context.MedicalRecords
                .Include(m => m.VitalSigns)
                .Include(m => m.Doctor)
                .ThenInclude(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<MedicalRecord?> GetByAppointmentIdAsync(int appointmentId)
        {
            return await _context.MedicalRecords
                .Include(m => m.VitalSigns)
                .Include(m => m.Doctor)
                .ThenInclude(d => d.User)
                .FirstOrDefaultAsync(m => m.AppointmentId == appointmentId);
        }

        public async Task<MedicalRecord> UpdateAsync(MedicalRecord medicalRecord)
        {
            _context.MedicalRecords.Update(medicalRecord);
            await _context.SaveChangesAsync();
            return medicalRecord;
        }

        public async Task<bool> ExistsByAppointmentIdAsync(int appointmentId)
        {
            return await _context.MedicalRecords
                .AnyAsync(m => m.AppointmentId == appointmentId);
        }
    }
}
