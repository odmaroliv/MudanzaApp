using System.Text.Json;

namespace MudanzaApp.Data.Services.Impl
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OpenAIService> _logger;

        public OpenAIService(HttpClient httpClient, IConfiguration configuration, ILogger<OpenAIService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;

            // Configurar el cliente HTTP
            _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configuration["OpenAI:ApiKey"]}");
        }

        public async Task<MudanzaItemAnalysisResult> AnalyzeItemImageAsync(byte[] imageData)
        {
            try
            {
                // Convertir la imagen a base64
                string base64Image = Convert.ToBase64String(imageData);

                // Crear el prompt para OpenAI
                // Instrucciones específicas para obtener solo el objeto principal en la imagen
                string systemPrompt = @"
                    Eres un asistente especializado en identificar artículos para mudanzas a partir de imágenes.
                    Tu tarea es identificar SOLO el objeto principal en la imagen y proporcionar:
                    1. El nombre del artículo (en singular, a menos que sean objetos que claramente van juntos)
                    2. Una breve descripción (1-2 líneas máximo)
                    3. La cantidad aproximada visible (si hay múltiples unidades del mismo artículo)
                    4. La categoría a la que pertenece (Muebles, Electrónicos, Ropa, Cocina, Decoración, Libros, Herramientas, Otro)
                    
                    IMPORTANTE: 
                    - Identifica SOLO UN tipo de artículo por imagen (el principal/dominante)
                    - No incluyas otros elementos del fondo
                    - No menciones a personas o mascotas
                    - No incluyas detalles sobre marcas, colores o estado (a menos que sea crucial)
                    - Responde SOLO con un objeto JSON con los campos 'name', 'description', 'quantity' y 'category'
                ";

                var requestContent = new
                {
                    model = "gpt-4-vision-preview",
                    messages = new object[]
                    {
                        new { role = "system", content = systemPrompt },
                        new {
                            role = "user",
                            content = new object[]
                            {
                                new { type = "text", text = "Identifica el artículo principal en esta imagen para mi mudanza." },
                                new {
                                    type = "image_url",
                                    image_url = new {
                                        url = $"data:image/jpeg;base64,{base64Image}"
                                    }
                                }
                            }
                        }
                    },
                    max_tokens = 300
                };

                var response = await _httpClient.PostAsJsonAsync("chat/completions", requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error from OpenAI API: {errorContent}");
                    return new MudanzaItemAnalysisResult
                    {
                        Success = false,
                        ErrorMessage = $"Error llamando a OpenAI: {response.StatusCode}"
                    };
                }

                var responseObject = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
                var content = responseObject.choices[0].message.content;

                // Intentar extraer el JSON de la respuesta
                try
                {
                    // Eliminar cualquier markdown de código si está presente
                    content = content.Replace("```json", "").Replace("```", "").Trim();

                    var itemData = JsonSerializer.Deserialize<MudanzaItemAnalysisResult>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return new MudanzaItemAnalysisResult
                    {
                        Success = true,
                        ItemName = itemData.ItemName ?? itemData.ItemName,  // Compatibilidad con diferentes formatos de respuesta
                        Description = itemData.Description ?? itemData.Description,
                        EstimatedQuantity = itemData.EstimatedQuantity,
                        Category = itemData.Category ?? itemData.Category
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error parsing OpenAI response: {ex.Message}. Response: {content}");
                    return new MudanzaItemAnalysisResult
                    {
                        Success = false,
                        ErrorMessage = "No se pudo analizar la respuesta de OpenAI"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling OpenAI service: {ex.Message}");
                return new MudanzaItemAnalysisResult
                {
                    Success = false,
                    ErrorMessage = $"Error: {ex.Message}"
                };
            }
        }

        // Clases para deserializar la respuesta de OpenAI
        private class OpenAIResponse
        {
            public Choice[] choices { get; set; }
        }

        private class Choice
        {
            public Message message { get; set; }
        }

        private class Message
        {
            public string content { get; set; }
        }
    }
}

