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

        public async Task<List<MedicalRecord>> GetByPatientIdAsync(int patientId)
        {
            return await _context.MedicalRecords
                .Include(m => m.VitalSigns)
                .Include(m => m.Doctor)
                .ThenInclude(d => d.User)
                .Where(m => m.PatientId == patientId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
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

        public async Task<List<MedicalRecord>> GetPendingAdmissionsAsync()
        {
            // Lấy PatientId của những bệnh nhân đang nằm viện (giường OCCUPIED)
            var occupiedPatientIds = await _context.Beds
                .Where(b => b.Status == "OCCUPIED" && b.CurrentPatientId.HasValue)
                .Select(b => b.CurrentPatientId!.Value)
                .ToListAsync();

            return await _context.MedicalRecords
                .Include(m => m.Patient)
                    .ThenInclude(p => p.User)
                .Include(m => m.Doctor)
                    .ThenInclude(d => d.User)
                .Where(m => m.IsHospitalized && !occupiedPatientIds.Contains(m.PatientId))
                .OrderByDescending(m => m.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
