using System.Text;
using MamaFit.Services.ExternalService.AI.Interface;
using MamaFit.Services.ExternalService.AI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MamaFit.Services.ExternalService.AI.Implements;

public class OllamaService : ILLMProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OllamaService> _logger;
        private readonly string _model;

        public OllamaService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<OllamaService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            
            var baseUrl = configuration["AI:Providers:Ollama:BaseUrl"];
            _model = configuration["AI:Providers:Ollama:Model"]!;
            
            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(60);
        }

        public async Task<string> GenerateResponseAsync(string prompt)
        {
            try
            {
                var request = new OllamaRequest
                {
                    Model = _model,
                    Prompt = $"You are a medical AI assistant. Respond ONLY with valid JSON, no other text.\n\n{prompt}",
                    Stream = false,
                    Options = new OllamaOptions
                    {
                        Temperature = 0.3f,
                        NumPredict = 600
                    }
                };

                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                _logger.LogDebug("Sending request to Ollama");
                
                var response = await _httpClient.PostAsync("/api/generate", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var ollamaResponse = JsonConvert.DeserializeObject<OllamaResponse>(responseContent);
                    
                    var result = ollamaResponse?.Response ?? "";
                    _logger.LogDebug($"Ollama response: {result}");
                    
                    return result;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Ollama API error: {response.StatusCode} - {error}");
                    throw new Exception($"Ollama API error: {response.StatusCode}. Content: {error}");

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Ollama API");
                throw;
            }
        }

        public async Task<bool> IsAvailable()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/tags");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(content);
                    
                    var models = result?.models;
                    if (models != null)
                    {
                        foreach (var model in models)
                        {
                            if (model.name.ToString().Contains(_model))
                            {
                                return true;
                            }
                        }
                    }
                    _logger.LogWarning($"Ollama model {_model} not found");
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking Ollama availability");
                return false;
            }
        }

        public string GetProviderName() => "Ollama";
    }