using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class AdmissionRepository : IAdmissionRepository
    {
        private readonly FirstAidContext _context;

        public AdmissionRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<AdmissionRecord> CreateAsync(AdmissionRecord record)
        {
            _context.AdmissionRecords.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<AdmissionRecord?> GetActiveAdmissionByPatientIdAsync(int patientId)
        {
            // Bệnh nhân đang nằm viện = chưa có ngày xuất viện
            return await _context.AdmissionRecords
                .Include(a => a.Bed)
                    .ThenInclude(b => b.Ward)
                .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                .Include(a => a.AdmittedByNurse)
                    .ThenInclude(n => n.User)
                .Include(a => a.VitalSigns)
                .FirstOrDefaultAsync(a => a.PatientId == patientId && a.DischargedAt == null);
        }

        public async Task<List<AdmissionRecord>> GetActiveAdmissionsAsync()
        {
            return await _context.AdmissionRecords
                .Include(a => a.Bed)
                    .ThenInclude(b => b.Ward)
                .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                .Include(a => a.MedicalRecord)
                .Include(a => a.VitalSigns)
                .Where(a => a.DischargedAt == null)
                .OrderByDescending(a => a.AdmittedAt)
                .ToListAsync();
        }

        public async Task<List<AdmissionRecord>> GetAllByPatientIdAsync(int patientId)
        {
            return await _context.AdmissionRecords
                .Include(a => a.Bed)
                    .ThenInclude(b => b.Ward)
                .Include(a => a.AdmittedByNurse)
                    .ThenInclude(n => n.User)
                .Include(a => a.MedicalRecord)
                    .ThenInclude(m => m.Doctor)
                        .ThenInclude(d => d.User)
                .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                .Include(a => a.VitalSigns)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AdmittedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(AdmissionRecord record)
        {
            _context.AdmissionRecords.Update(record);
            await _context.SaveChangesAsync();
        }
    }
}
