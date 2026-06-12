using FirstAidAPI.Data;
using FirstAidAPI.DTO.Ward;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class WardRepository : IWardRepository
    {
        private readonly FirstAidContext _context;

        public WardRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<List<Ward>> GetBySpecialtyAsync(int specialtyId)
        {
            return await _context.Wards
                .Where(w => w.SpecialityId == specialtyId)
                .ToListAsync();
        }

        public async Task<List<Ward>> GetAllWardsAsync()
        {
            return await _context.Wards
                .Include(w => w.Speciality)
                .Include(w => w.Beds)
                .OrderBy(w => w.Floor).ThenBy(w => w.RoomNumber)
                .ToListAsync();
        }

        public async Task<Ward?> GetWardByIdAsync(int id)
        {
            return await _context.Wards
                .Include(w => w.Beds)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Ward> CreateWardAsync(Ward ward)
        {
            _context.Wards.Add(ward);
            await _context.SaveChangesAsync();
            return ward;
        }

        public async Task UpdateWardAsync(Ward ward)
        {
            _context.Wards.Update(ward);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWardAsync(int id)
        {
            var ward = await _context.Wards.FindAsync(id);
            if (ward != null)
            {
                _context.Wards.Remove(ward);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<AdmissionRecord>> GetActiveAdmissionsByRoomAsync(string roomNumber)
        {
            return await _context.AdmissionRecords
                .Include(a => a.Patient)
                .Include(a => a.Bed).ThenInclude(b => b.Ward)
                .Include(a => a.MedicalRecord).ThenInclude(mr => mr.VitalSigns)
                .Include(a => a.MedicalRecord).ThenInclude(mr => mr.Doctor).ThenInclude(d => d.User)
                .Include(a => a.AdmittedByNurse).ThenInclude(n => n.User)
                .Where(a => a.DischargedAt == null && a.Bed.Ward.RoomNumber == roomNumber)
                .OrderBy(a => a.Bed.BedNumber)
                .ToListAsync();
        }

        public async Task<List<WardRoomSummaryDto>> GetRoomSummariesAsync(List<int>? allowedWardIds = null)
        {
            var query = _context.Wards.AsQueryable();

            if (allowedWardIds != null)
            {
                query = query.Where(w => allowedWardIds.Contains(w.Id));
            }

            return await query
                .Select(w => new WardRoomSummaryDto
                {
                    RoomNumber = w.RoomNumber,
                    WardType = w.WardType,
                    Floor = w.Floor,
                    TotalBeds = w.Beds.Count,
                    PatientCount = w.Beds.Count(b => b.Status == "OCCUPIED")
                })
                .OrderBy(r => r.Floor).ThenBy(r => r.RoomNumber)
                .ToListAsync();
        }

        public async Task<List<int>> GetAssignedWardIdsForDoctorAsync(int doctorId, DateOnly date)
        {
            return await _context.DoctorSchedules
                .Where(s => s.DoctorId == doctorId && s.Date == date && s.WardId != null && !s.IsOff)
                .Select(s => s.WardId!.Value)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<int>> GetAssignedWardIdsForNurseAsync(int nurseId, DateOnly date)
        {
            return await _context.NurseSchedules
                .Where(s => s.NurseId == nurseId && s.Date == date && s.WardId != null && !s.IsOff)
                .Select(s => s.WardId!.Value)
                .Distinct()
                .ToListAsync();
        }

        // ─── Ward Orders ─────────────────────────────────────────────────────────

        public async Task<WardOrder?> GetOrderByIdAsync(int id)
        {
            return await _context.WardOrders
                .Include(wo => wo.CreatedByDoctor).ThenInclude(d => d.User)
                .FirstOrDefaultAsync(wo => wo.Id == id);
        }

        public async Task<List<WardOrder>> GetOrdersByAdmissionAsync(int admissionRecordId)
        {
            return await _context.WardOrders
                .Include(wo => wo.CreatedByDoctor).ThenInclude(d => d.User)
                .Where(wo => wo.AdmissionRecordId == admissionRecordId)
                .OrderByDescending(wo => wo.CreatedAt)
                .ToListAsync();
        }

        public async Task<WardOrder> CreateOrderAsync(WardOrder order)
        {
            _context.WardOrders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task UpdateOrderAsync(WardOrder order)
        {
            _context.WardOrders.Update(order);
            await _context.SaveChangesAsync();
        }

        // ─── Ward Notes ──────────────────────────────────────────────────────────

        public async Task<List<WardNote>> GetNotesByAdmissionAsync(int admissionRecordId)
        {
            return await _context.WardNotes
                .Include(wn => wn.Author)
                .Where(wn => wn.AdmissionRecordId == admissionRecordId)
                .OrderByDescending(wn => wn.CreatedAt)
                .ToListAsync();
        }

        public async Task<WardNote> CreateNoteAsync(WardNote note)
        {
            _context.WardNotes.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }

        // ─── Vital Signs ─────────────────────────────────────────────────────

        public async Task<List<VitalSign>> GetVitalsHistoryByAdmissionAsync(int admissionRecordId)
        {
            var admission = await _context.AdmissionRecords
                .FirstOrDefaultAsync(a => a.Id == admissionRecordId);

            if (admission == null) return new List<VitalSign>();

            return await _context.VitalSigns
                .Include(v => v.Nurse).ThenInclude(n => n.User)
                .Include(v => v.MedicalRecord).ThenInclude(mr => mr.Doctor).ThenInclude(d => d.User)
                .Where(v => v.AdmissionRecordId == admissionRecordId || v.MedicalRecordId == admission.MedicalRecordId)
                .OrderByDescending(v => v.RecordedAt)
                .ToListAsync();
        }

        public async Task<VitalSign> CreateVitalSignAsync(VitalSign vitalSign)
        {
            _context.VitalSigns.Add(vitalSign);
            await _context.SaveChangesAsync();
            return vitalSign;
        }
    }
}
