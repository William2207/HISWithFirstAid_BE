using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using FirstAidAPI.DTO.Payment;

namespace FirstAidAPI.Service.Payment
{
    public class MomoService : IMomoService
    {
        private readonly IConfiguration _configuration;

        public MomoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<MomoCreatePaymentResponseDto> CreatePaymentAsync(MomoCreatePaymentRequestDto request)
        {
            var endpoint = _configuration["Momo:Endpoint"];
            var partnerCode = _configuration["Momo:PartnerCode"];
            var accessKey = _configuration["Momo:AccessKey"];
            var secretKey = _configuration["Momo:SecretKey"] ?? throw new InvalidOperationException("Momo:SecretKey is missing in configuration."); ;
            var returnUrl = _configuration["Momo:ReturnUrl"];
            var ipnUrl = _configuration["Momo:IpnUrl"];
            var requestType = _configuration["Momo:RequestType"];

            var orderId = request.OrderNumber;
            var amount = request.Amount;
            var orderInfo = request.OrderDescription;
            var redirectUrl = returnUrl;
            var ipnUrls = ipnUrl;
            var requestId = Guid.NewGuid().ToString();
            var extraData = "";

            // Tạo chữ ký
            var rawHash = $"accessKey={accessKey}" +
                          $"&amount={amount}" +
                          $"&extraData={extraData}" +
                          $"&ipnUrl={ipnUrls}" +
                          $"&orderId={orderId}" +
                          $"&orderInfo={orderInfo}" +
                          $"&partnerCode={partnerCode}" +
                          $"&redirectUrl={redirectUrl}" +
                          $"&requestId={requestId}" +
                          $"&requestType={requestType}";

            var signature = ComputeHmacSha256(rawHash, secretKey);

            // Tạo request body
            var requestBody = new
            {
                partnerCode = partnerCode,
                partnerName = "MoMo Payment",
                storeId = "Test Store",
                requestId = requestId,
                amount = amount,
                orderId = orderId,
                orderInfo = orderInfo,
                redirectUrl = redirectUrl,
                ipnUrl = ipnUrls,
                lang = "vi",
                extraData = extraData,
                requestType = requestType,
                signature = signature
            };

            // Gọi API Momo
            using var httpClient = new HttpClient();
            var content = new StringContent(
                JsonConvert.SerializeObject(requestBody),
                Encoding.UTF8,
                "application/json"
            );
            //var jsonRequest = JsonConvert.SerializeObject(requestBody, Formatting.Indented);
            //Console.WriteLine("===== REQUEST GỬI LÊN MOMO =====");
            //Console.WriteLine(jsonRequest);
            //Console.WriteLine("================================");

            var response = await httpClient.PostAsync(endpoint, content);
            var responseString = await response.Content.ReadAsStringAsync();

            //Console.WriteLine("===== RESPONSE TỪ MOMO =====");
            //Console.WriteLine(responseString);
            //Console.WriteLine("============================");
            //Console.WriteLine("===== DEBUG SIGNATURE =====");
            //Console.WriteLine($"AccessKey: {accessKey}");
            //Console.WriteLine($"SecretKey: {secretKey?.Substring(0, 5)}...");  // Chỉ show 5 ký tự đầu
            //Console.WriteLine($"RawHash: {rawHash}");
            //Console.WriteLine($"Signature: {signature}");
            //Console.WriteLine("===========================");
            var momoResponse = JsonConvert.DeserializeObject<MomoCreatePaymentResponseDto>(responseString)
                ?? throw new InvalidOperationException("Cannot deserialize MoMo response.");

            return momoResponse;
        }

        public bool ValidateSignature(MomoCallbackDto callback)
        {
            var secretKey = _configuration["Momo:SecretKey"]
                ?? throw new InvalidOperationException("Momo:SecretKey is missing in configuration.");
            var accessKey = _configuration["Momo:AccessKey"]
                ?? throw new InvalidOperationException("Momo:AccessKey is missing in configuration.");

            var rawHash = $"accessKey={accessKey}" +
                          $"&amount={callback.Amount}" +
                          $"&extraData={callback.ExtraData}" +
                          $"&message={callback.Message}" +
                          $"&orderId={callback.OrderId}" +
                          $"&orderInfo={callback.OrderInfo}" +
                          $"&orderType={callback.OrderType}" +
                          $"&partnerCode={callback.PartnerCode}" +
                          $"&payType={callback.PayType}" +
                          $"&requestId={callback.RequestId}" +
                          $"&responseTime={callback.ResponseTime}" +
                          $"&resultCode={callback.ResultCode}" +
                          $"&transId={callback.TransId}";

            var signature = ComputeHmacSha256(rawHash, secretKey);

            return signature.Equals(callback.Signature, StringComparison.OrdinalIgnoreCase);
        }

        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using var hmac = new HMACSHA256(keyBytes);
            var hashBytes = hmac.ComputeHash(messageBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}