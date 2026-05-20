using FirstAidAPI.DTO.Ward;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;

namespace FirstAidAPI.Service.Implement
{
    public class WardService : IWardService
    {
        private readonly IWardRepository _wardRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly INurseRepository _nurseRepository;

        public WardService(
            IWardRepository wardRepository,
            IDoctorRepository doctorRepository,
            INurseRepository nurseRepository)
        {
            _wardRepository = wardRepository;
            _doctorRepository = doctorRepository;
            _nurseRepository = nurseRepository;
        }

        public async Task<List<WardRoomSummaryDto>> GetRoomSummariesAsync(int userId, string role)
        {
            List<int>? allowedWardIds = null;
            var today = DateOnly.FromDateTime(DateTime.Now);

            if (role == "Doctor")
            {
                var doctor = await _doctorRepository.GetByUserIdAsync(userId);
                if (doctor != null)
                {
                    allowedWardIds = await _wardRepository.GetAssignedWardIdsForDoctorAsync(doctor.Id, today);
                }
            }
            else if (role == "Nurse")
            {
                var nurse = await _nurseRepository.GetByUserIdAsync(userId);
                if (nurse != null)
                {
                    allowedWardIds = await _wardRepository.GetAssignedWardIdsForNurseAsync(nurse.Id, today);
                }
            }

            return await _wardRepository.GetRoomSummariesAsync(allowedWardIds);
        }

        public async Task<List<WardPatientDto>> GetPatientsByRoomAsync(string roomNumber)
        {
            var admissions = await _wardRepository.GetActiveAdmissionsByRoomAsync(roomNumber);
            var dtoList = new List<WardPatientDto>();

            foreach (var a in admissions)
            {
                var vitalsList = await _wardRepository.GetVitalsHistoryByAdmissionAsync(a.Id);
                var latest = vitalsList.FirstOrDefault();

                dtoList.Add(new WardPatientDto
                {
                    AdmissionRecordId = a.Id,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FullNameDisplay,
                    BedNumber = a.Bed.BedNumber,
                    RoomNumber = a.Bed.Ward.RoomNumber,
                    WardType = a.Bed.Ward.WardType,
                    AdmittedAt = a.AdmittedAt,
                    PatientAge = a.Patient.DateOfBirth.HasValue
                        ? DateTime.Now.Year - a.Patient.DateOfBirth.Value.Year
                        : 0,
                    PatientGender = a.Patient.Gender,
                    DiagnosisName = a.MedicalRecord?.DiagnosisName,
                    DoctorName = a.MedicalRecord?.Doctor?.User?.FullName,
                    LatestVitals = latest != null ? MapVitalSignToDto(latest) : null
                });
            }

            return dtoList;
        }

        // ─── Orders ──────────────────────────────────────────────────────────────

        public async Task<List<WardOrderDto>> GetOrdersByAdmissionAsync(int admissionRecordId)
        {
            var orders = await _wardRepository.GetOrdersByAdmissionAsync(admissionRecordId);
            return orders.Select(MapOrderToDto).ToList();
        }

        public async Task<WardOrderDto> CreateOrderAsync(int doctorUserId, CreateWardOrderRequest request)
        {
            var doctor = await _doctorRepository.GetByUserIdAsync(doctorUserId)
                ?? throw new InvalidOperationException("Doctor profile not found for the current user.");

            var order = new WardOrder
            {
                AdmissionRecordId = request.AdmissionRecordId,
                CreatedByDoctorId = doctor.Id,
                OrderType = request.OrderType,
                Title = request.Title,
                Description = request.Description,
                Status = "PENDING",
                ScheduledAt = request.ScheduledAt,
                CreatedAt = DateTime.UtcNow
            };

            var saved = await _wardRepository.CreateOrderAsync(order);
            // Reload with navigation properties
            var full = await _wardRepository.GetOrderByIdAsync(saved.Id)
                       ?? throw new InvalidOperationException("Could not load saved order.");
            return MapOrderToDto(full);
        }

        public async Task<WardOrderDto> UpdateOrderAsync(int orderId, int doctorUserId, UpdateWardOrderRequest request)
        {
            var order = await _wardRepository.GetOrderByIdAsync(orderId)
                ?? throw new KeyNotFoundException($"Ward order {orderId} not found.");

            // Only the doctor who created the order can edit it
            var doctor = await _doctorRepository.GetByUserIdAsync(doctorUserId)
                ?? throw new InvalidOperationException("Doctor profile not found for the current user.");

            if (order.CreatedByDoctorId != doctor.Id)
                throw new UnauthorizedAccessException("You can only edit orders you created.");

            if (order.Status == "COMPLETED" || order.Status == "CANCELLED")
                throw new InvalidOperationException($"Cannot edit an order with status '{order.Status}'.");

            order.OrderType = request.OrderType;
            order.Title = request.Title;
            order.Description = request.Description;
            order.ScheduledAt = request.ScheduledAt;
            order.UpdatedAt = DateTime.UtcNow;

            await _wardRepository.UpdateOrderAsync(order);

            var full = await _wardRepository.GetOrderByIdAsync(orderId)
                       ?? throw new InvalidOperationException("Could not reload updated order.");
            return MapOrderToDto(full);
        }

        public async Task<WardOrderDto> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _wardRepository.GetOrderByIdAsync(orderId)
                ?? throw new KeyNotFoundException($"Ward order {orderId} not found.");

            order.Status = newStatus;
            if (newStatus == "COMPLETED")
                order.CompletedAt = DateTime.UtcNow;

            await _wardRepository.UpdateOrderAsync(order);

            var full = await _wardRepository.GetOrderByIdAsync(orderId)!
                       ?? throw new InvalidOperationException("Could not reload updated order.");
            return MapOrderToDto(full);
        }

        // ─── Notes ───────────────────────────────────────────────────────────────

        public async Task<List<WardNoteDto>> GetNotesByAdmissionAsync(int admissionRecordId)
        {
            var notes = await _wardRepository.GetNotesByAdmissionAsync(admissionRecordId);
            return notes.Select(MapNoteToDto).ToList();
        }

        public async Task<WardNoteDto> CreateNoteAsync(int authorUserId, string authorRole, CreateWardNoteRequest request)
        {
            var note = new WardNote
            {
                AdmissionRecordId = request.AdmissionRecordId,
                AuthorUserId = authorUserId,
                AuthorRole = authorRole,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow
            };

            var saved = await _wardRepository.CreateNoteAsync(note);

            // Reload with Author navigation
            var notes = await _wardRepository.GetNotesByAdmissionAsync(request.AdmissionRecordId);
            var full = notes.FirstOrDefault(n => n.Id == saved.Id) ?? saved;
            return MapNoteToDto(full);
        }

        // ─── Private mappers ─────────────────────────────────────────────────────

        private static WardOrderDto MapOrderToDto(WardOrder wo) => new()
        {
            Id = wo.Id,
            AdmissionRecordId = wo.AdmissionRecordId,
            CreatedByDoctorId = wo.CreatedByDoctorId,
            DoctorName = wo.CreatedByDoctor?.User?.FullName ?? string.Empty,
            OrderType = wo.OrderType,
            Title = wo.Title,
            Description = wo.Description,
            Status = wo.Status,
            CreatedAt = wo.CreatedAt,
            UpdatedAt = wo.UpdatedAt,
            ScheduledAt = wo.ScheduledAt,
            CompletedAt = wo.CompletedAt
        };

        private static WardNoteDto MapNoteToDto(WardNote wn) => new()
        {
            Id = wn.Id,
            AdmissionRecordId = wn.AdmissionRecordId,
            AuthorName = wn.Author?.FullName ?? string.Empty,
            AuthorRole = wn.AuthorRole,
            Content = wn.Content,
            CreatedAt = wn.CreatedAt
        };

        // ─── Vital Signs ─────────────────────────────────────────────────────

        public async Task<List<VitalsDto>> GetVitalsHistoryByAdmissionAsync(int admissionRecordId)
        {
            var list = await _wardRepository.GetVitalsHistoryByAdmissionAsync(admissionRecordId);
            return list.Select(MapVitalSignToDto).ToList();
        }

        public async Task<VitalsDto> LogVitalsAsync(int? nurseUserId, int admissionRecordId, LogVitalsRequest request)
        {
            int? nurseId = null;
            if (nurseUserId.HasValue)
            {
                var nurse = await _nurseRepository.GetByUserIdAsync(nurseUserId.Value);
                if (nurse != null)
                {
                    nurseId = nurse.Id;
                }
            }

            var vitalSign = new VitalSign
            {
                AdmissionRecordId = admissionRecordId,
                NurseId = nurseId,
                HeartRate = request.HeartRate,
                BloodPressure = request.BloodPressure,
                Temperature = request.Temperature,
                SpO2 = request.SpO2,
                RespiratoryRate = request.RespiratoryRate,
                Weight = request.Weight,
                Height = request.Height,
                RecordedAt = DateTime.UtcNow
            };

            var saved = await _wardRepository.CreateVitalSignAsync(vitalSign);

            var history = await _wardRepository.GetVitalsHistoryByAdmissionAsync(admissionRecordId);
            var full = history.FirstOrDefault(v => v.Id == saved.Id) ?? saved;
            return MapVitalSignToDto(full);
        }

        private static VitalsDto MapVitalSignToDto(VitalSign vs) => new()
        {
            Id = vs.Id,
            HeartRate = vs.HeartRate,
            BloodPressure = vs.BloodPressure,
            Temperature = vs.Temperature,
            SpO2 = vs.SpO2,
            RespiratoryRate = vs.RespiratoryRate,
            Weight = vs.Weight,
            Height = vs.Height,
            RecordedAt = vs.RecordedAt,
            RecordedBy = vs.Nurse != null
                ? $"ĐD. {vs.Nurse.User?.FullName ?? ""}"
                : (vs.MedicalRecord?.Doctor != null ? $"BS. {vs.MedicalRecord.Doctor.User?.FullName ?? ""}" : "Hệ thống")
        };
    }
}
