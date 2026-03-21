using FirstAidAPI.DTO.Appointment;
using FirstAidAPI.Enums;
using FirstAidAPI.Exceptions;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;

namespace FirstAidAPI.Service.Implement
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IUserRepository _userRepository; // For patient/doctor checks if needed

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IMedicalRecordRepository medicalRecordRepository,
            IInvoiceRepository invoiceRepository,
            IUserRepository userRepository)
        {
            _appointmentRepository = appointmentRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _invoiceRepository = invoiceRepository;
            _userRepository = userRepository;
        }

        public async Task<AppointmentDTO> CreateAppointmentAsync(int creatorId, CreateAppointmentRequest request)
        {
            // 1. Create Appointment
            var appointment = new Appointment
            {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                SpecialtyId = request.SpecialtyId,
                ClinicId = request.ClinicId,
                AppointmentDateTime = request.AppointmentDateTime,
                Type = AppointmentType.WalkIn, // For receptionist, it's usually WalkIn
                Status = AppointmentStatus.Registered, // Started as Registered/Waiting
            };

            var savedAppointment = await _appointmentRepository.AddAsync(appointment);

            // 2. Auto-create empty Medical Record
            var emptyMedicalRecord = new MedicalRecord
            {
                AppointmentId = savedAppointment.Id,
                DoctorId = request.DoctorId,
                CreatedAt = DateTime.UtcNow
            };
            await _medicalRecordRepository.CreateAsync(emptyMedicalRecord);

            return await GetAppointmentByIdAsync(savedAppointment.Id);
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

        public async Task<AppointmentDTO> CompleteAppointmentAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null) throw new NotFoundException($"Không tìm thấy lịch hẹn có id {appointmentId}");

            appointment.Status = AppointmentStatus.Completed;
            await _appointmentRepository.UpdateAsync(appointment);

            // Invoice logic
            var invoice = new Invoice
            {
                AppointmentId = appointment.Id,
                PatientId = appointment.PatientId,
                InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{appointment.Id:D4}",
                Status = "UNPAID",
                CreatedAt = DateTime.UtcNow
            };
            await _invoiceRepository.AddAsync(invoice);
            
            return MapToDTO(appointment);
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
                SpecialtyName = appointment.Specialty?.Name ?? string.Empty
            };
        }
    }
}
