using System.Text;
using MamaFit.Services.ExternalService.AI.Interface;
using MamaFit.Services.ExternalService.AI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MamaFit.Services.ExternalService.AI.Implements;

public class GroqService : ILLMProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GroqService> _logger;
        private readonly string _apiKey;
        private readonly string _model;

        public GroqService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<GroqService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            
            _apiKey = configuration["AI:Providers:Groq:ApiKey"] ?? "";
            _model = configuration["AI:Providers:Groq:Model"] ?? "mixtral-8x7b-32768";
            
            if (!string.IsNullOrEmpty(_apiKey))
            {
                _httpClient.BaseAddress = new Uri("https://api.groq.com/openai/v1/");
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            }
        }

        public async Task<string> GenerateResponseAsync(string prompt)
        {
            try
            {
                var request = new Model.LLMRequest
                {
                    Model = _model,
                    Messages = new List<Model.LLMMessage>
                    {
                        new Model.LLMMessage 
                        { 
                            Role = "system", 
                            Content = "You are a medical AI assistant specializing in pregnancy measurements. Always respond with valid JSON only, no additional text."
                        },
                        new Model.LLMMessage 
                        { 
                            Role = "user", 
                            Content = prompt 
                        }
                    },
                    Temperature = 0.3f,
                    MaxTokens = 600
                };

                var json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                });
                
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                _logger.LogDebug($"Sending request to Groq API");
                
                var response = await _httpClient.PostAsync("chat/completions", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var groqResponse = JsonConvert.DeserializeObject<Model.GroqResponse>(responseContent);
                    
                    var result = groqResponse?.Choices?.FirstOrDefault()?.Message.Content ?? "";
                    _logger.LogDebug($"Groq response: {result}");
                    
                    return result;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Groq API error: {response.StatusCode} - {error}");
                    throw new Exception($"Groq API error: {response.StatusCode}. Content: {error}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Groq API");
                throw;
            }
        }

        public async Task<bool> IsAvailable()
        {
            try
            {
                if (string.IsNullOrEmpty(_apiKey))
                {
                    return false;
                }

                // Simple health check
                var testRequest = new
                {
                    model = _model,
                    messages = new[] 
                    { 
                        new { role = "user", content = "test" } 
                    },
                    max_tokens = 5
                };

                var json = JsonConvert.SerializeObject(testRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync("chat/completions", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking Groq availability");
                return false;
            }
        }

        public string GetProviderName() => "Groq";
    }