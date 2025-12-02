using FirstAidAPI.DTO.Payment;

namespace FirstAidAPI.Service.Payment
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseDto> CreatePaymentAsync(MomoCreatePaymentRequestDto request);

        bool ValidateSignature(MomoCallbackDto callback);
    }
}