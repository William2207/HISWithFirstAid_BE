using FirstAidAPI.Repository;
using FirstAidAPI.Service;
using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using System.Text;
using System.Text.Json;

namespace FirstAidAPI.Service.Implement
{
    public class PatientSummaryService : IPatientSummaryService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IAdmissionService _admissionService;
        private readonly IConfiguration _configuration;

        public PatientSummaryService(
            IPatientRepository patientRepository,
            IMedicalRecordService medicalRecordService,
            IAdmissionService admissionService,
            IConfiguration configuration)
        {
            _patientRepository = patientRepository;
            _medicalRecordService = medicalRecordService;
            _admissionService = admissionService;
            _configuration = configuration;
        }

        public async Task<string> SummarizePatientAsync(int patientId)
        {
            // 1. Fetch data
            var patient = await _patientRepository.GetByIdAsync(patientId);
            if (patient == null)
            {
                throw new Exception("Patient not found");
            }

            var user = patient.User;
            var medicalRecords = await _medicalRecordService.GetMedicalRecordsByPatientAsync(patientId);
            var admissions = await _admissionService.GetAdmissionHistoryByPatientIdAsync(patientId);

            // 2. Build prompt
            var sb = new StringBuilder();
            sb.AppendLine("Bạn là một trợ lý y tế AI chuyên nghiệp. Dưới đây là thông tin hồ sơ của một bệnh nhân, bao gồm thông tin cá nhân, các chỉ số sinh tồn, lịch sử khám bệnh và lịch sử nhập viện. Hãy đọc và tạo ra một bản tóm tắt bệnh án chuyên nghiệp, rõ ràng, tập trung vào các vấn đề y tế chính, chẩn đoán, và hướng điều trị. Hãy định dạng bằng Markdown.");

            sb.AppendLine("\n### THÔNG TIN BỆNH NHÂN");
            sb.AppendLine($"- Họ và tên: {user?.FullName ?? "Không rõ"}");
            sb.AppendLine($"- Tuổi: {(user != null && user.DateOfBirth.Year > 1900 ? (DateTime.Now.Year - user.DateOfBirth.Year).ToString() : "Không rõ")}");
            sb.AppendLine($"- Giới tính: {user?.Gender ?? "Không rõ"}");
            sb.AppendLine($"- Nhóm máu: {patient.BloodType ?? "Không rõ"}");
            sb.AppendLine($"- Dị ứng: {patient.Allergies ?? "Không có"}");
            sb.AppendLine($"- Tiền sử bệnh: {patient.MedicalHistory ?? "Không rõ"}");

            sb.AppendLine("\n### LỊCH SỬ KHÁM BỆNH (Các lần khám gần nhất)");
            if (!medicalRecords.Any())
            {
                sb.AppendLine("Không có dữ liệu khám bệnh.");
            }
            else
            {
                // Take top 5 recent records
                foreach (var record in medicalRecords.OrderByDescending(m => m.CreatedAt).Take(5))
                {
                    sb.AppendLine($"- Ngày: {record.CreatedAt:dd/MM/yyyy HH:mm}");
                    sb.AppendLine($"  + Chẩn đoán: {record.DiagnosisName ?? "Không rõ"}");
                    sb.AppendLine($"  + Triệu chứng chính: {record.ChiefComplaint ?? record.Symptoms ?? "Không rõ"}");
                    if (!string.IsNullOrEmpty(record.MedicalHistory))
                        sb.AppendLine($"  + Tiền sử bản thân: {record.MedicalHistory}");
                    if (!string.IsNullOrEmpty(record.FamilyHistory))
                        sb.AppendLine($"  + Tiền sử gia đình: {record.FamilyHistory}");
                    sb.AppendLine($"  + Hướng điều trị: {record.TreatmentPlan ?? "Không có"}");
                    if (record.NextAppointmentDate.HasValue)
                        sb.AppendLine($"  + Lịch hẹn tiếp theo: {record.NextAppointmentDate.Value:dd/MM/yyyy HH:mm}");
                    if (!string.IsNullOrEmpty(record.Prescription))
                    {
                        sb.AppendLine($"  + Đơn thuốc: {FormatPrescription(record.Prescription)}");
                    }
                }
            }

            sb.AppendLine("\n### LỊCH SỬ NHẬP VIỆN");
            if (!admissions.Any())
            {
                sb.AppendLine("Không có dữ liệu nhập viện.");
            }
            else
            {
                foreach (var admission in admissions.OrderByDescending(a => a.AdmittedAt).Take(3))
                {
                    var dischargeStr = admission.DischargedAt.HasValue ? admission.DischargedAt.Value.ToString("dd/MM/yyyy") : "Đang nằm viện";
                    sb.AppendLine($"- Từ {admission.AdmittedAt:dd/MM/yyyy} đến {dischargeStr}");
                    sb.AppendLine($"  + Chẩn đoán: {admission.DiagnosisName ?? "Không rõ"}");
                    sb.AppendLine($"  + Lý do: {admission.ChiefComplaint ?? "Không rõ"}");
                    sb.AppendLine($"  + Phác đồ điều trị: {admission.TreatmentPlan ?? "Không có"}");
                }
            }

            sb.AppendLine("\n### YÊU CẦU TÓM TẮT:");
            sb.AppendLine("Dựa vào thông tin trên, hãy viết một đoạn tóm tắt bệnh án ngắn gọn (khoảng 150-250 từ), nêu bật tình trạng sức khỏe tổng quát, các bệnh lý mãn tính (nếu có), các vấn đề y tế gần đây nhất, và tóm tắt quá trình điều trị. Không tự bịa thêm thông tin ngoài những gì được cung cấp.");

            var prompt = sb.ToString();

            // 3. Call Gemini AI
            var apiKey = _configuration["Gemini:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("Gemini API key is not configured.");
            }

            var googleAi = new GoogleAI(apiKey);
            var model = googleAi.GenerativeModel("gemini-2.5-flash"); // Or Gemini 1.5 Flash

            var response = await model.GenerateContent(prompt);
            return response.Text;
        }

        private string FormatPrescription(string prescriptionJson)
        {
            try
            {
                // Attempt to parse json
                var meds = JsonSerializer.Deserialize<List<MedicationDto>>(prescriptionJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (meds != null && meds.Any())
                {
                    return string.Join(", ", meds.Select(m => $"{m.Name} ({m.Dosage})"));
                }
            }
            catch
            {
                // Ignore parsing errors, return plain text
            }
            return prescriptionJson;
        }

        private class MedicationDto
        {
            public string Name { get; set; } = string.Empty;
            public string Dosage { get; set; } = string.Empty;
        }
    }
}
