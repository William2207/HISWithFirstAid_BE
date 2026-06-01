using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ChatController> _logger;

        public ChatController(HttpClient httpClient, IConfiguration configuration, ILogger<ChatController> _logger)
        {
            this._httpClient = httpClient;
            this._configuration = configuration;
            this._logger = _logger;
        }

        [HttpPost("query")]
        public async Task<IActionResult> QueryRag([FromBody] ChatQueryRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new { error = "Message is required." });
            }

            try
            {
                var ragBaseUrl = _configuration["RagSettings:BaseUrl"] ?? "http://localhost:9621";
                var url = $"{ragBaseUrl.TrimEnd('/')}/query";

                var payload = new
                {
                    query = request.Message,
                    mode = "hybrid"
                };

                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                _logger.LogInformation("Forwarding chat query to RAG server: {Url}", url);
                
                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("RAG server returned error status {StatusCode}: {ErrorContent}", response.StatusCode, errorContent);
                    return StatusCode((int)response.StatusCode, new { error = "The medical assistant service returned an error. Please try again later." });
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                
                // Parse the response to ensure we return a clean JSON payload
                using var jsonDoc = JsonDocument.Parse(jsonResponse);
                
                // If the response from LightRAG is in the format { "response": "..." }, return it directly
                if (jsonDoc.RootElement.TryGetProperty("response", out var responseProperty))
                {
                    return Ok(new { response = responseProperty.GetString() });
                }

                // If not, return the whole response object as is
                return Ok(jsonDoc.RootElement);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP exception occurred while connecting to the RAG server.");
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { error = "Cannot connect to the medical assistant service. Make sure the RAG server is running." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred in ChatController.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred while processing your request." });
            }
        }
    }

    public class ChatQueryRequest
    {
        public string Message { get; set; }
    }
}
