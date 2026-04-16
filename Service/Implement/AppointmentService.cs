using FirstAidAPI.Data;
using FirstAidAPI.DTO.Appointment;
using FirstAidAPI.DTO.Invoice;
using FirstAidAPI.DTO.Order;
using FirstAidAPI.DTO.Payment;
using FirstAidAPI.Enums;
using FirstAidAPI.Exceptions;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;
using FirstAidAPI.Service.Payment;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Service.Implement
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly ISpecialtyRepository _specialtyRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMomoService _momoService;
        private readonly FirstAidContext _context;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IMedicalRecordRepository medicalRecordRepository,
            IInvoiceRepository invoiceRepository,
            IPatientRepository patientRepository,
            ISpecialtyRepository specialtyRepository,
            IDoctorRepository doctorRepository,
            IMomoService momoService,
            UserManager<User> userManager,
            FirstAidContext context,
            IUserRepository userRepository,
            ILogger<AppointmentService> logger)
        {
            _appointmentRepository = appointmentRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _invoiceRepository = invoiceRepository;
            _patientRepository = patientRepository;
            _specialtyRepository = specialtyRepository;
            _doctorRepository = doctorRepository;
            _momoService = momoService;
            _userManager = userManager;
            _context = context;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<AppointmentDTO> CreateAppointmentAsync(int creatorId, CreateAppointmentRequest request)
        {
            var creator = await _userRepository.GetByIdAsync(creatorId);
            if (creator == null)
                throw new NotFoundException($"Không tìm thấy người dùng có ID {creatorId}");

            var roles = await _userManager.GetRolesAsync(creator);

            if (request.AppointmentDateTime <= DateTime.UtcNow)
                throw new BusinessException("Thời gian hẹn phải ở tương lai.");

            int resolvedPatientId;
            AppointmentType appointmentType;

            if (roles.Contains("Receptionist"))
            {
                resolvedPatientId = await ResolvePatientForReceptionistAsync(request);
                appointmentType = AppointmentType.WalkIn;
            }
            else
            {
                var existing = await _patientRepository.GetByUserIdAsync(creatorId);
                if (existing == null)
                    throw new NotFoundException($"Không tìm thấy hồ sơ bệnh nhân cho người dùng ID {creatorId}");

                resolvedPatientId = existing.Id;
                appointmentType = AppointmentType.Online;
            }

            // Kiểm tra trùng lịch hẹn (Same Patient, Same Specialty, Same Time)
            var isDuplicate = await _appointmentRepository.ExistsOverlapAsync(resolvedPatientId, request.SpecialtyId, request.AppointmentDateTime);
            if (isDuplicate)
            {
                throw new BusinessException("Bạn đã có lịch hẹn cho chuyên khoa này vào khung giờ đã chọn. Vui lòng kiểm tra lại trong lịch sử đặt lịch.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var dayOfWeek = request.AppointmentDateTime.DayOfWeek;
                var timeOfDay = request.AppointmentDateTime.TimeOfDay;

                var schedule = await _doctorRepository.GetScheduleAsync(request.DoctorId, dayOfWeek, timeOfDay);

                if (schedule != null)
                {
                    if (appointmentType == AppointmentType.Online)
                    {
                        if (schedule.MaxOnlineSlots <= 0)
                            throw new BusinessException("Đã hết slot đặt hẹn đăng ký online cho khung giờ này.");
                        schedule.MaxOnlineSlots--;
                    }
                    else
                    {
                        if (schedule.MaxWalkInSlots <= 0)
                            throw new BusinessException("Đã hết slot đặt hẹn đăng ký trực tiếp cho khung giờ này.");
                        schedule.MaxWalkInSlots--;
                    }
                    await _doctorRepository.UpdateScheduleAsync(schedule);
                }

                var appointment = new Appointment
                {
                    PatientId = resolvedPatientId,
                    DoctorId = request.DoctorId,
                    SpecialtyId = request.SpecialtyId,
                    ClinicId = request.ClinicId,
                    CreatorId = creatorId,
                    AppointmentDateTime = request.AppointmentDateTime,
                    Type = appointmentType,
                    Status = AppointmentStatus.Registered,
                };

                var savedAppointment = await _appointmentRepository.AddAsync(appointment);
                await transaction.CommitAsync();

                return await GetAppointmentByIdAsync(savedAppointment.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<AppointmentDTO> GetAppointmentByIdAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null) throw new NotFoundException($"Không tìm thấy lịch hẹn có id {id}");
            return MapToDTO(appointment);
        }

        public async Task<IEnumerable<AppointmentDTO>> GetWaitingAppointmentsByDoctorAsync(int doctorId)
        {
            var appointments = await _appointmentRepository.GetWaitingAppointmentsByDoctorAsync(doctorId);
            return appointments.Select(MapToDTO);
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAppointmentsByDoctorAndDateAsync(int doctorId, DateTime date)
        {
            var appointments = await _appointmentRepository.GetAppointmentsByDoctorAndDateAsync(doctorId, date);
            return appointments.Select(MapToDTO);
        }

        public async Task<AppointmentDTO> CompleteAppointmentAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null) throw new NotFoundException($"Không tìm thấy lịch hẹn có id {appointmentId}");

            appointment.Status = AppointmentStatus.Completed;
            await _appointmentRepository.UpdateAsync(appointment);

            // Bác sĩ hoàn tất khám (không tự động tạo Invoice, bộ phận tiếp đón/thu ngân sẽ lo việc này cho walk-in, hoặc online đã thanh toán)
            return MapToDTO(appointment);
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAppointmentsByPatientAsync(int patientId)
        {
            var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId);
            return appointments.Select(MapToDTO);
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAppointmentsByUserIdAsync(int userId)
        {
            var patient = await _patientRepository.GetByUserIdAsync(userId);
            if (patient == null)
            {
                throw new NotFoundException("Không tìm thấy hồ sơ bệnh nhân.");
            }

            return await GetAppointmentsByPatientAsync(patient.Id);
        }

        public async Task<IEnumerable<AppointmentDTO>> GetCompletedAppointmentsAsync()
        {
            var appointments = await _appointmentRepository.GetCompletedAppointmentsAsync();
            return appointments.Select(MapToDTO);
        }

        public async Task CancelAppointmentAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                throw new NotFoundException($"Không tìm thấy lịch hẹn có id {appointmentId}");

            _logger.LogInformation("Cancelling appointment {AppointmentId}", appointmentId);
            await _appointmentRepository.DeleteAsync(appointment);
        }

        public async Task<AppointmentDTO> StartAppointmentAsync(int appointmentId, int doctorId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                throw new NotFoundException($"Không tìm thấy lịch hẹn có id {appointmentId}");

            if (appointment.DoctorId != doctorId)
                throw new BusinessException("Bác sĩ không có quyền tiếp nhận lịch hẹn này.");

            if (appointment.Status != AppointmentStatus.Registered)
                throw new BusinessException($"Lịch hẹn đang ở trạng thái '{appointment.Status}', không thể bắt đầu.");

            appointment.Status = AppointmentStatus.In_Progress;
            await _appointmentRepository.UpdateAsync(appointment);

            // Auto-tạo MedicalRecord trống nếu chưa có
            var alreadyHasRecord = await _medicalRecordRepository.ExistsByAppointmentIdAsync(appointmentId);
            if (!alreadyHasRecord)
            {
                var emptyRecord = new MedicalRecord
                {
                    AppointmentId = appointment.Id,
                    DoctorId = doctorId,
                    PatientId = appointment.PatientId,
                    CreatedAt = DateTime.UtcNow
                };
                await _medicalRecordRepository.CreateAsync(emptyRecord);
                _logger.LogInformation(
                    "Auto-created empty MedicalRecord for Appointment {AppointmentId}", appointmentId);
            }

            return await GetAppointmentByIdAsync(appointmentId);
        }

        private async Task<int> ResolvePatientForReceptionistAsync(CreateAppointmentRequest request)
        {
            // Bệnh nhân đã có CCCD → tìm hoặc tạo mới
            if (!string.IsNullOrWhiteSpace(request.IdCard))
            {
                var existing = await _patientRepository.GetByIdCardAsync(request.IdCard);
                if (existing != null)
                    return existing.Id;
            }

            // Không có CCCD hoặc chưa tồn tại → tạo bệnh nhân vãng lai
            var walkIn = BuildWalkInPatient(request);
            var saved = await _patientRepository.AddAsync(walkIn);
            return saved.Id;
        }

        private Patient BuildWalkInPatient(CreateAppointmentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
                throw new BusinessException("Vui lòng nhập họ tên bệnh nhân vãng lai.");

            DateTime? utcDateOfBirth = request.DateOfBirth.HasValue
                ? DateTime.SpecifyKind(request.DateOfBirth.Value, DateTimeKind.Utc)
                : null;

            return new Patient
            {
                FullName = request.FullName,
                DateOfBirth = utcDateOfBirth,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                InsuranceNumber = request.InsuranceNumber,
                IdCard = request.IdCard,
                CreatedAt = DateTime.UtcNow
            };
        }

        private static AppointmentDTO MapToDTO(Appointment appointment)
        {
            return new AppointmentDTO
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                SpecialtyId = appointment.SpecialtyId,
                ClinicId = appointment.ClinicId,
                AppointmentDateTime = appointment.AppointmentDateTime,
                Type = appointment.Type,
                Status = appointment.Status,
                PatientName = appointment.Patient?.FullNameDisplay ?? string.Empty,
                DoctorName = appointment.Doctor?.User?.FullName ?? string.Empty,
                SpecialtyName = appointment.Specialty?.Name ?? string.Empty,
                MedicalRecordId = appointment.MedicalRecord?.Id
            };
        }
    }
}
