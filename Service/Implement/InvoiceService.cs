using FirstAidAPI.Repository;
using FirstAidAPI.DTO.Invoice;
using FirstAidAPI.DTO.Payment;
using FirstAidAPI.Models;
using FirstAidAPI.Enums;
using FirstAidAPI.Service.Payment;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace FirstAidAPI.Service.Implement
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ILabOrderRepository _labOrderRepository;
        private readonly IMomoService _momoService;
        private readonly IPatientRepository _patientRepository;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<InvoiceService> _logger;

        public InvoiceService(
            IInvoiceRepository invoiceRepository,
            IPaymentRepository paymentRepository,
            IAppointmentRepository appointmentRepository,
            ILabOrderRepository labOrderRepository,
            IMomoService momoService,
            IPatientRepository patientRepository,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<InvoiceService> logger)
        {
            _invoiceRepository = invoiceRepository;
            _paymentRepository = paymentRepository;
            _appointmentRepository = appointmentRepository;
            _labOrderRepository = labOrderRepository;
            _momoService = momoService;
            _patientRepository = patientRepository;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task<CreateInvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceDto dto)
        {
            // Xác định loại invoice: Appointment hoặc LabOrder
            if (dto.LabOrderId.HasValue)
                return await CreateLabOrderInvoiceAsync(dto);

            return await CreateAppointmentInvoiceAsync(dto);
        }

        private async Task<CreateInvoiceResponseDto> CreateAppointmentInvoiceAsync(CreateInvoiceDto dto)
        {
            if (dto.AppointmentId == null)
                throw new ArgumentException("AppointmentId là bắt buộc.");

            var appointment = await _appointmentRepository.GetByIdAsync(dto.AppointmentId.Value)
                ?? throw new KeyNotFoundException("Không tìm thấy lịch hẹn.");

            var existingInvoice = await _invoiceRepository.GetByAppointmentIdAsync(dto.AppointmentId.Value);
            if (existingInvoice != null)
                throw new InvalidOperationException(
                    $"Hóa đơn đã tồn tại cho lịch hẹn này (Số: {existingInvoice.InvoiceNumber}).");

            decimal subtotal = appointment.Specialty?.Price ?? 0;
            decimal total = subtotal - dto.Discount;
            if (total < 0)
                throw new ArgumentException("Giảm giá không thể vượt quá tổng tiền.");

            var invoiceNumber = GenerateInvoiceNumber();
            var invoice = new Invoice
            {
                AppointmentId = dto.AppointmentId,
                PatientId = appointment.PatientId,
                InvoiceNumber = invoiceNumber,
                Subtotal = subtotal,
                Discount = dto.Discount,
                Total = total,
                RemainingAmount = total,
                Status = OrderStatus.Pending
            };

            await _invoiceRepository.AddAsync(invoice);

            return await ProcessPaymentAsync(invoice, dto.PaymentMethod,
                $"Thanh toán hóa đơn khám bệnh {invoiceNumber}",
                dto.Discount, subtotal);
        }

        private async Task<CreateInvoiceResponseDto> CreateLabOrderInvoiceAsync(CreateInvoiceDto dto)
        {
            var labOrder = await _labOrderRepository.GetByIdAsync(dto.LabOrderId!.Value)
                ?? throw new KeyNotFoundException($"Không tìm thấy chỉ định {dto.LabOrderId.Value}.");

            if (labOrder.Status != LabOrderStatus.Pending)
                throw new InvalidOperationException("Chỉ có thể tạo hóa đơn cho chỉ định đang ở trạng thái Chờ thanh toán.");

            var existingInvoice = await _invoiceRepository.GetByLabOrderIdAsync(dto.LabOrderId.Value);
            if (existingInvoice != null)
                throw new InvalidOperationException(
                    $"Hóa đơn đã tồn tại cho chỉ định này (Số: {existingInvoice.InvoiceNumber}).");

            decimal subtotal = labOrder.Items.Sum(i => i.Amount);
            decimal total = subtotal - dto.Discount;
            if (total < 0)
                throw new ArgumentException("Giảm giá không thể vượt quá tổng tiền.");

            var invoiceNumber = GenerateInvoiceNumber();
            var invoice = new Invoice
            {
                LabOrderId = dto.LabOrderId,
                PatientId = labOrder.PatientId,
                InvoiceNumber = invoiceNumber,
                Subtotal = subtotal,
                Discount = dto.Discount,
                Total = total,
                RemainingAmount = total,
                Status = OrderStatus.Pending
            };

            await _invoiceRepository.AddAsync(invoice);

            string? paymentUrl = null;

            if (dto.PaymentMethod == PaymentMethod.Cash)
            {
                invoice.Status = OrderStatus.Completed;
                invoice.PaidAmount = total;
                invoice.RemainingAmount = 0;
                invoice.PaidAt = DateTime.UtcNow;
                await _invoiceRepository.UpdateAsync(invoice);

                await CreatePaymentRecordAsync(invoice, PaymentMethod.Cash,
                    "CASH-" + Guid.NewGuid().ToString("N")[..8].ToUpper());

                // Cập nhật trạng thái LabOrder → Paid
                labOrder.Status = LabOrderStatus.Paid;
                await _labOrderRepository.UpdateAsync(labOrder);

                _logger.LogInformation(
                    "LabOrder Invoice (Cash) completed. InvoiceNumber: {InvoiceNumber}, LabOrderId: {LabOrderId}, Total: {Total}",
                    invoiceNumber, labOrder.Id, total);

                TriggerMockLabOrderResult(labOrder.Id);
            }
            else
            {
                var baseUrl = _momoService.GetBaseUrl();
                var momoResponse = await _momoService.CreatePaymentAsync(new MomoCreatePaymentRequestDto
                {
                    OrderNumber = invoiceNumber,
                    Amount = (long)total,
                    OrderDescription = $"Thanh toán chỉ định xét nghiệm {invoiceNumber}",
                    ReturnUrl = $"{baseUrl}/api/invoices/momo-callback"
                });

                if (momoResponse.ResultCode == 0)
                    paymentUrl = momoResponse.PayUrl;

                _logger.LogInformation(
                    "LabOrder Invoice (Momo) created. InvoiceNumber: {InvoiceNumber}, LabOrderId: {LabOrderId}, Total: {Total}",
                    invoiceNumber, labOrder.Id, total);
            }

            return new CreateInvoiceResponseDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                Subtotal = subtotal,
                Discount = dto.Discount,
                Total = total,
                Status = invoice.Status,
                PaymentUrl = paymentUrl
            };
        }

        private async Task<CreateInvoiceResponseDto> ProcessPaymentAsync(
            Invoice invoice, PaymentMethod? paymentMethod,
            string momoDescription, decimal discount, decimal subtotal)
        {
            string? paymentUrl = null;

            if (paymentMethod == PaymentMethod.Cash)
            {
                invoice.Status = OrderStatus.Completed;
                invoice.PaidAmount = invoice.Total;
                invoice.RemainingAmount = 0;
                invoice.PaidAt = DateTime.UtcNow;
                await _invoiceRepository.UpdateAsync(invoice);

                await CreatePaymentRecordAsync(invoice, PaymentMethod.Cash,
                    "CASH-" + Guid.NewGuid().ToString("N")[..8].ToUpper());

                _logger.LogInformation(
                    "Cash Invoice completed. InvoiceNumber: {InvoiceNumber}, PatientId: {PatientId}, Total: {Total}",
                    invoice.InvoiceNumber, invoice.PatientId, invoice.Total);
            }
            else
            {
                var baseUrl = _momoService.GetBaseUrl();
                var momoResponse = await _momoService.CreatePaymentAsync(new MomoCreatePaymentRequestDto
                {
                    OrderNumber = invoice.InvoiceNumber,
                    Amount = (long)invoice.Total,
                    OrderDescription = momoDescription,
                    ReturnUrl = $"{baseUrl}/api/invoices/momo-callback"
                });

                if (momoResponse.ResultCode == 0)
                    paymentUrl = momoResponse.PayUrl;

                _logger.LogInformation(
                    "Momo Invoice created. InvoiceNumber: {InvoiceNumber}, PatientId: {PatientId}, Total: {Total}",
                    invoice.InvoiceNumber, invoice.PatientId, invoice.Total);
            }

            return new CreateInvoiceResponseDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                Subtotal = subtotal,
                Discount = discount,
                Total = invoice.Total,
                Status = invoice.Status,
                PaymentUrl = paymentUrl
            };
        }

        private async Task CreatePaymentRecordAsync(Invoice invoice, PaymentMethod method, string transactionId)
        {
            var payment = new Models.Payment
            {
                InvoiceId = invoice.Id,
                PatientId = invoice.PatientId,
                Amount = invoice.Total,
                PaymentMethod = method,
                Status = PaymentStatus.Completed,
                TransactionId = transactionId,
                PaidAt = DateTime.UtcNow
            };
            await _paymentRepository.AddAsync(payment);
        }

        public async Task CompleteInvoiceAsync(int invoiceId, string transactionId)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId)
                ?? throw new KeyNotFoundException($"Invoice {invoiceId} not found.");

            // Kiểm tra invoice đã hoàn tất chưa (Idempotency)
            if (invoice.Status == OrderStatus.Completed)
            {
                _logger.LogInformation("Invoice {InvoiceNumber} already completed.", invoice.InvoiceNumber);
                return;
            }

            // Cập nhật LabOrder.Status → Paid nếu invoice liên kết với LabOrder
            if (invoice.LabOrderId.HasValue)
            {
                var labOrder = await _labOrderRepository.GetByIdAsync(invoice.LabOrderId.Value);
                if (labOrder != null && labOrder.Status == LabOrderStatus.Pending)
                {
                    labOrder.Status = LabOrderStatus.Paid;
                    await _labOrderRepository.UpdateAsync(labOrder);

                    TriggerMockLabOrderResult(labOrder.Id);
                }
            }

            // Kiểm tra payment đã tồn tại chưa
            var existingPayment = await _paymentRepository.GetByTransactionIdAsync(transactionId);
            if (existingPayment != null)
            {
                _logger.LogWarning("Payment already exists for transaction {TransactionId}. Marking invoice as completed if not already.", transactionId);

                // Đảm bảo sync trạng thái invoice nếu cần
                invoice.Status = OrderStatus.Completed;
                await _invoiceRepository.UpdateAsync(invoice);
                return;
            }

            // Tạo Payment record
            var payment = new Models.Payment
            {
                InvoiceId = invoice.Id,
                PatientId = invoice.PatientId,
                Amount = invoice.Total,
                PaymentMethod = PaymentMethod.Momo,
                Status = PaymentStatus.Completed,
                TransactionId = transactionId,
                PaidAt = DateTime.UtcNow
            };

            await _paymentRepository.AddAsync(payment);

            // Update Invoice status
            invoice.Status = OrderStatus.Completed;
            invoice.PaidAmount = invoice.Total;
            invoice.RemainingAmount = 0;
            invoice.PaidAt = DateTime.UtcNow;

            await _invoiceRepository.UpdateAsync(invoice);

            _logger.LogInformation("Invoice completed. InvoiceNumber: {InvoiceNumber}, TransactionId: {TransactionId}, Amount: {Amount}",
                invoice.InvoiceNumber, transactionId, invoice.Total);
        }

        public async Task FailInvoiceAsync(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId)
                ?? throw new KeyNotFoundException($"Invoice {invoiceId} not found.");

            if (invoice.Status != OrderStatus.Pending)
            {
                _logger.LogWarning("Cannot fail invoice {InvoiceNumber}. Current status: {Status}",
                    invoice.InvoiceNumber, invoice.Status);
                throw new InvalidOperationException($"Only pending invoices can be failed.");
            }

            invoice.Status = OrderStatus.Cancelled;
            await _invoiceRepository.UpdateAsync(invoice);

            // Hủy appointment khi payment fail
            if (invoice.AppointmentId.HasValue)
            {
                var appointment = await _appointmentRepository.GetByIdAsync(invoice.AppointmentId.Value);
                if (appointment != null)
                {
                    await _appointmentRepository.DeleteAsync(appointment);
                    _logger.LogInformation("Cancelled appointment {AppointmentId} due to failed invoice {InvoiceNumber}",
                        invoice.AppointmentId.Value, invoice.InvoiceNumber);
                }
            }

            _logger.LogInformation("Invoice failed. InvoiceNumber: {InvoiceNumber}", invoice.InvoiceNumber);
        }

        // Được gọi từ InvoicesController sau khi lookup bằng InvoiceNumber
        public async Task<CreateInvoiceResponseDto?> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            var invoice = await _invoiceRepository.GetByInvoiceNumberAsync(invoiceNumber);
            if (invoice == null) return null;

            return new CreateInvoiceResponseDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                Subtotal = invoice.Subtotal,
                Discount = invoice.Discount,
                Total = invoice.Total,
                Status = invoice.Status
            };
        }

        public async Task<IEnumerable<PatientInvoiceDto>> GetInvoicesByPatientAsync(int patientId)
        {
            var invoices = await _invoiceRepository.GetByPatientIdAsync(patientId);

            return invoices.Select(i => new PatientInvoiceDto
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                Description = i.Appointment?.Specialty?.Name ?? "Thanh toán dịch vụ y tế",
                Total = i.Total,
                Status = i.Status,
                CreatedAt = i.CreatedAt,
                PaidAt = i.PaidAt,
                PaymentMethod = i.Payments.OrderByDescending(p => p.PaidAt).FirstOrDefault()?.PaymentMethod.ToString() ?? "Chưa có",
            }).ToList();
        }

        public async Task<IEnumerable<PatientInvoiceDto>> GetInvoicesByUserIdAsync(int userId)
        {
            var patient = await _patientRepository.GetByUserIdAsync(userId);
            if (patient == null)
            {
                throw new KeyNotFoundException("Không tìm thấy hồ sơ bệnh nhân.");
            }

            return await GetInvoicesByPatientAsync(patient.Id);
        }

        public async Task ProcessMomoPaymentAsync(MomoCallbackDto callback)
        {
            var invoice = await _invoiceRepository.GetByInvoiceNumberAsync(callback.OrderId);
            if (invoice == null)
            {
                _logger.LogError("Invoice {InvoiceNumber} not found during Momo callback processing.", callback.OrderId);
                throw new KeyNotFoundException($"Invoice {callback.OrderId} not found.");
            }

            if (callback.ResultCode == 0)
            {
                // Thanh toán thành công -> Hoàn tất hóa đơn và tạo Payment record
                await CompleteInvoiceAsync(invoice.Id, callback.TransId.ToString());
            }
            else
            {
                // Thanh toán thất bại -> Hủy hóa đơn
                _logger.LogWarning("Payment failed for Invoice {InvoiceNumber}. ResultCode: {ResultCode}, Message: {Message}",
                    invoice.InvoiceNumber, callback.ResultCode, callback.Message);

                invoice.Status = OrderStatus.Cancelled;
                await _invoiceRepository.UpdateAsync(invoice);

                // Hủy appointment liên quan nếu có
                if (invoice.AppointmentId.HasValue)
                {
                    var appointment = await _appointmentRepository.GetByIdAsync(invoice.AppointmentId.Value);
                    if (appointment != null)
                    {
                        await _appointmentRepository.DeleteAsync(appointment);
                        _logger.LogInformation("Deleted appointment {AppointmentId} due to failed payment for invoice {InvoiceNumber}",
                            invoice.AppointmentId.Value, invoice.InvoiceNumber);
                    }
                }
            }
        }

        public string GenerateInvoiceNumber()
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string uniquePart = Guid.NewGuid().ToString("N")[..8].ToUpper();
            return $"INV-{date}-{uniquePart}";
        }

        private void TriggerMockLabOrderResult(int labOrderId)
        {
            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(4000); // Giả lập chờ 4 giây thực hiện

                    using var scope = _serviceScopeFactory.CreateScope();
                    var labOrderRepo = scope.ServiceProvider.GetRequiredService<ILabOrderRepository>();

                    var labOrder = await labOrderRepo.GetByIdAsync(labOrderId);
                    if (labOrder != null && labOrder.Status == LabOrderStatus.Paid)
                    {
                        labOrder.Status = LabOrderStatus.Completed;
                        foreach (var item in labOrder.Items)
                        {
                            var serviceName = item.MedicalService?.Name ?? "";
                            ApplyMockResult(item, serviceName);
                        }
                        await labOrderRepo.UpdateAsync(labOrder);
                        _logger.LogInformation("Mocked result for LabOrder {LabOrderId}", labOrderId);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error mocking lab order result for {LabOrderId}", labOrderId);
                }
            });
        }

        private static void ApplyMockResult(LabOrderItem item, string serviceName)
        {
            var note = item.Note ?? "";

            // ── Nhóm Hình ảnh ─────────────────────────────────────────────
            if (serviceName.Contains("X-quang", StringComparison.OrdinalIgnoreCase))
            {
                ApplyXRayResult(item, note);
                return;
            }
            if (serviceName.Contains("Siêu âm", StringComparison.OrdinalIgnoreCase))
            {
                ApplyUltrasoundResult(item, note);
                return;
            }
            if (serviceName.Contains("CT", StringComparison.OrdinalIgnoreCase))
            {
                ApplyCtScanResult(item, note);
                return;
            }
            if (serviceName.Contains("MRI", StringComparison.OrdinalIgnoreCase))
            {
                ApplyMriResult(item, note);
                return;
            }
            if (serviceName.Contains("Nội soi dạ dày", StringComparison.OrdinalIgnoreCase))
            {
                item.ResultImageUrl = "https://tieuhoaviet.com.vn/wp-content/uploads/2025/12/hinh-anh-noi-soi-da-day-bi-ung-thu-1.jpg";
                item.ResultNote = "Niêm mạc thực quản, dạ dày và tá tràng bình thường. Không có loét, polyp hay khối u. (Mock)";
                return;
            }
            if (serviceName.Contains("Nội soi đại tràng", StringComparison.OrdinalIgnoreCase))
            {
                item.ResultImageUrl = "https://suckhoedoisong.qltns.mediacdn.vn/324455921873985536/2023/5/15/1-16841240404001085578864.jpg";
                item.ResultNote = "Niêm mạc đại tràng hồng đều, không có polyp, loét hay khối u. (Mock)";
                return;
            }

            // ── Nhóm Xét nghiệm (Lab) ─────────────────────────────────────
            if (serviceName.Contains("máu", StringComparison.OrdinalIgnoreCase))
            {
                item.ResultNote = "Xét nghiệm công thức máu toàn bộ (CBC). (Mock)";
                item.ResultData = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { name = "Hồng cầu (RBC)", value = "4.85", unit = "T/L",  refRange = "4.2 – 5.4",   isAbnormal = false },
                    new { name = "Bạch cầu (WBC)", value = "11.2",  unit = "G/L",  refRange = "4.5 – 11.0",  isAbnormal = true  },
                    new { name = "Tiểu cầu (PLT)", value = "215",   unit = "G/L",  refRange = "150 – 400",   isAbnormal = false },
                    new { name = "Hemoglobin (HGB)", value = "138",  unit = "g/L",  refRange = "120 – 160",   isAbnormal = false },
                    new { name = "Hematocrit (HCT)", value = "41.2", unit = "%",    refRange = "37 – 47",     isAbnormal = false },
                    new { name = "Bạch cầu đa nhân (Neutrophil)", value = "72", unit = "%", refRange = "50 – 70", isAbnormal = true }
                });
                return;
            }
            if (serviceName.Contains("nước tiểu", StringComparison.OrdinalIgnoreCase))
            {
                item.ResultNote = "Xét nghiệm tổng phân tích nước tiểu (Urinalysis). (Mock)";
                item.ResultData = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { name = "pH",              value = "6.5",      unit = "",  refRange = "4.5 – 8.0",    isAbnormal = false },
                    new { name = "Protein",         value = "Âm tính",  unit = "",  refRange = "Âm tính",      isAbnormal = false },
                    new { name = "Glucose",         value = "Âm tính",  unit = "",  refRange = "Âm tính",      isAbnormal = false },
                    new { name = "Ketone",          value = "Âm tính",  unit = "",  refRange = "Âm tính",      isAbnormal = false },
                    new { name = "Bạch cầu niệu",  value = "++",       unit = "",  refRange = "Âm tính",      isAbnormal = true  },
                    new { name = "Hồng cầu niệu",  value = "Âm tính",  unit = "",  refRange = "Âm tính",      isAbnormal = false },
                    new { name = "Tỷ trọng",        value = "1.018",    unit = "",  refRange = "1.010 – 1.025", isAbnormal = false }
                });
                return;
            }
            if (serviceName.Contains("ECG", StringComparison.OrdinalIgnoreCase) ||
                serviceName.Contains("Điện tim", StringComparison.OrdinalIgnoreCase))
            {
                item.ResultNote = "Kết quả điện tâm đồ (ECG 12 chuyển đạo). (Mock)";
                item.ResultData = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { name = "Nhịp tim (HR)",      value = "78",           unit = "lần/phút", refRange = "60 – 100",    isAbnormal = false },
                    new { name = "Nhịp",               value = "Xoang đều",   unit = "",         refRange = "Xoang đều",   isAbnormal = false },
                    new { name = "Trục điện tim",      value = "Bình thường", unit = "",         refRange = "Bình thường", isAbnormal = false },
                    new { name = "PR Interval",        value = "160",         unit = "ms",       refRange = "120 – 200",   isAbnormal = false },
                    new { name = "QRS Duration",       value = "88",          unit = "ms",       refRange = "< 120",       isAbnormal = false },
                    new { name = "QTc",                value = "420",         unit = "ms",       refRange = "< 440",       isAbnormal = false }
                });
                return;
            }
            if (serviceName.Contains("vaccine", StringComparison.OrdinalIgnoreCase) ||
                serviceName.Contains("Tiêm", StringComparison.OrdinalIgnoreCase))
            {
                item.ResultNote = "Hoàn tất tiêm chủng. (Mock)";
                item.ResultData = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    new { name = "Tên vaccine",         value = "Cúm mùa (Influvac Tetra)", unit = "", refRange = "", isAbnormal = false },
                    new { name = "Số lô",               value = "FL2026-A412",              unit = "", refRange = "", isAbnormal = false },
                    new { name = "Liều tiêm",           value = "0.5 mL",                  unit = "", refRange = "", isAbnormal = false },
                    new { name = "Vị trí tiêm",         value = "Cơ delta trái",           unit = "", refRange = "", isAbnormal = false },
                    new { name = "Phản ứng sau tiêm",   value = "Không có",                unit = "", refRange = "Không", isAbnormal = false }
                });
                return;
            }

            // ── Mặc định (fallback) ────────────────────────────────────────
            item.ResultImageUrl = "https://images.unsplash.com/photo-1579684385127-1ef15d508118?q=80&w=1920&auto=format&fit=crop";
            item.ResultNote = "Kết quả bình thường. (Mock)";
        }

        // ── X-Quang: phân loại theo bộ phận trong Note ────────────────────
        private static void ApplyXRayResult(LabOrderItem item, string note)
        {
            bool Is(params string[] keywords) =>
                keywords.Any(k => note.Contains(k, StringComparison.OrdinalIgnoreCase));

            if (Is("đầu", "sọ", "não", "hộp sọ"))
            {
                item.ResultImageUrl = "https://nganhanh.com.vn/upload/filemanager/chup-x-quang-dau-la-gi.jpg";
                item.ResultNote = "X-Quang sọ não: Không thấy tổn thương xương sọ, không có gãy, không có khối choán chỗ. (Mock)";
            }
            else if (Is("tay", "cánh tay", "bàn tay", "cổ tay", "cẳng tay", "khuỷu tay"))
            {
                item.ResultImageUrl = "https://cdn.nhathuoclongchau.com.vn/unsafe/800x0/chup_x_quang_xuong_cang_tay_thang_nghieng_la_ky_thuat_gi_chi_dinh_va_quy_trinh_1_db0a02d43d.png";
                item.ResultNote = "X-Quang chi trên: Cấu trúc xương bình thường. Không thấy gãy xương hay trật khớp. (Mock)";
            }
            else if (Is("chân", "bàn chân", "cổ chân", "cẳng chân", "đùi", "gối", "khớp gối"))
            {
                item.ResultImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQIozKc-iiM2d6qPM7IleG1nauhba3NqD92ZCHsELQA-NBLGZKcIEerKUcV&s=10";
                item.ResultNote = "X-Quang chi dưới: Cấu trúc xương khớp bình thường. Không thấy gãy, không hẹp khe khớp. (Mock)";
            }
            else if (Is("cột sống", "lưng", "thắt lưng", "cổ", "đốt sống"))
            {
                item.ResultImageUrl = "https://login.medlatec.vn//ImagePath/images/20230426/20230426_chup-x-quang-cot-song-3.png";
                item.ResultNote = "X-Quang cột sống: Các đốt sống thẳng hàng. Không hẹp đĩa đệm, không thấy thoát vị. (Mock)";
            }
            else if (Is("bụng", "ổ bụng", "bụng dưới"))
            {
                item.ResultImageUrl = "https://cdn.nhathuoclongchau.com.vn/unsafe/800x0/chup_x_quang_bung_khong_chuan_bi_va_nhung_dieu_ban_can_biet2_2d9e92970a.jpg";
                item.ResultNote = "X-Quang bụng: Không thấy hình ảnh liềm hơi, quai ruột bình thường. (Mock)";
            }
            else // Mặc định: Phổi / Ngực
            {
                item.ResultImageUrl = "https://xray.vn/wp-content/uploads/2016/09/u-1.png";
                item.ResultNote = "X-Quang ngực thẳng: Phổi 2 bên thông thoáng. Tim và trung thất bình thường. (Mock)";
            }
        }

        // ── Siêu âm: phân loại theo bộ phận trong Note ────────────────────
        private static void ApplyUltrasoundResult(LabOrderItem item, string note)
        {
            bool Is(params string[] keywords) =>
                keywords.Any(k => note.Contains(k, StringComparison.OrdinalIgnoreCase));

            if (Is("tim", "cardiac", "tim mạch"))
            {
                item.ResultImageUrl = "https://thietbiytethienphuc.com/uploads/images/images/hinh-anh-sieu-am-tim.jpg";
                item.ResultNote = "Siêu âm tim: Chức năng co bóp thất trái bình thường (EF ≈ 65%). Không có tràn dịch màng ngoài tim. (Mock)";
            }
            else if (Is("tuyến giáp", "giáp", "cổ"))
            {
                item.ResultImageUrl = "https://www.vinmec.com/static/uploads/small_20201208_090430_825122_suy_than_man_max_1800x1800_jpg_c8cd60b90f.jpg";
                item.ResultNote = "Siêu âm tuyến giáp: Thùy phải và thùy trái đều đặn, không có nhân. (Mock)";
            }
            else if (Is("thai", "tử cung", "buồng trứng", "phụ khoa"))
            {
                item.ResultImageUrl = "https://hongngochospital.vn/wp-content/uploads/2020/02/sieu-am-thai-1.jpg";
                item.ResultNote = "Siêu âm phụ khoa: Tử cung và 2 buồng trứng bình thường. Không có dịch tự do. (Mock)";
            }
            else // Mặc định: Bụng tổng quát
            {
                item.ResultImageUrl = "https://cdn.prod.website-files.com/5c93193a199a685a12dd8142/62d3c22049432b2341079290_11-hinh-anh-sieu-am-o-bung-o-nam-va-nu2.jpg";
                item.ResultNote = "Siêu âm bụng tổng quát: Gan, túi mật, lách, tụy, thận 2 bên hình thái bình thường. Không có dịch ổ bụng. (Mock)";
            }
        }

        // ── CT Scanner: phân loại theo bộ phận trong Note ─────────────────
        private static void ApplyCtScanResult(LabOrderItem item, string note)
        {
            bool Is(params string[] keywords) =>
                keywords.Any(k => note.Contains(k, StringComparison.OrdinalIgnoreCase));

            if (Is("ngực", "phổi", "lồng ngực"))
            {
                item.ResultImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRwEca7vbP2oQw6Ook1pkKXr4A6dz1KfXVgcNsTu4YxMi4VkuSGFyw34vQ&s=10";
                item.ResultNote = "CT Ngực: Nhu mô phổi 2 bên không thấy tổn thương. Không có tràn khí, tràn dịch màng phổi. (Mock)";
            }
            else if (Is("bụng", "ổ bụng", "gan", "tụy", "thận"))
            {
                item.ResultImageUrl = "https://www.touchstoneimaging.com/wp-content/uploads/2024/01/AdobeStock_213100426-768x577.jpeg";
                item.ResultNote = "CT Bụng: Các cơ quan ổ bụng bình thường. Không có tổn thương khu trú, không có dịch tự do. (Mock)";
            }
            else if (Is("cột sống", "lưng", "thắt lưng", "cổ"))
            {
                item.ResultImageUrl = "https://medlatec.vn/media/3755/content/20221107_lumbar-spine-ct-scan_thumb.jpg";
                item.ResultNote = "CT Cột sống: Đường cong sinh lý bình thường. Không hẹp ống sống, không thoát vị đĩa đệm rõ. (Mock)";
            }
            else // Mặc định: Não / Đầu
            {
                item.ResultImageUrl = "https://login.medlatec.vn//ckfinder/userfiles/images/chup-ct-dau-1.jpg";
                item.ResultNote = "CT Não: Không thấy bất thường cấu trúc nội sọ. Hệ thống não thất không giãn. Không có xuất huyết. (Mock)";
            }
        }

        // ── MRI: phân loại theo bộ phận trong Note ────────────────────────
        private static void ApplyMriResult(LabOrderItem item, string note)
        {
            bool Is(params string[] keywords) =>
                keywords.Any(k => note.Contains(k, StringComparison.OrdinalIgnoreCase));

            if (Is("cột sống", "lưng", "thắt lưng", "cổ", "đốt sống"))
            {
                item.ResultImageUrl = "https://www.vinmec.com/static/uploads/small_20200602_082037_764350_chup_mri_cot_song_t_max_1800x1800_jpg_e7b6239411.jpg";
                item.ResultNote = "MRI Cột sống: Không thấy thoát vị đĩa đệm đáng kể. Tủy sống tín hiệu bình thường. (Mock)";
            }
            else if (Is("gối", "khớp gối", "chân", "vai", "khớp vai", "tay"))
            {
                item.ResultImageUrl = "https://bernard.vn/static/5207/2025/12/29/mceu_58601381611767003376975.jpg";
                item.ResultNote = "MRI Cơ Xương Khớp: Sụn khớp còn tốt. Dây chằng và gân nguyên vẹn, không có rách. (Mock)";
            }
            else if (Is("ngực", "tim", "trung thất"))
            {
                item.ResultImageUrl = "https://benhvienphuongdong.vn/public/uploads/2024/thang-1/chup-mri-tim/chup-mri-tim-1.jpg";
                item.ResultNote = "MRI Ngực/Tim: Hình thái và chức năng tim bình thường. Không có tổn thương cơ tim. (Mock)";
            }
            else // Mặc định: Não
            {
                item.ResultImageUrl = "https://benhvienphuongdong.vn/public/uploads/2024/thang-1/chup-mri-mach-mau-nao-mra-1/chup-mri-mach-mau-nao-mra-3.jpeg";
                item.ResultNote = "MRI Não: Tín hiệu nhu mô não đồng nhất. Không có ổ nhồi máu, xuất huyết hay u. (Mock)";
            }
        }
    }
}
