using MVC.Services.Interfaces;
using Shared.DTOs;
using System.Text.Json;

namespace MVC.Services
{
    public class DepartamentoService : IDepartamentoService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;

        public DepartamentoService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory?.CreateClient("ApiClient") ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<IEnumerable<DepartamentoDTO>> GetDepartamentosAsync()
        {
            var response = await _httpClient.GetAsync($"/api/departamento");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Erro ao obter departamentos: {response.ReasonPhrase}");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<DepartamentoDTO>>(jsonString, _serializerOptions) ?? Enumerable.Empty<DepartamentoDTO>();
        }

        public async Task<DepartamentoDTO> GetDepartamentoAsync(string codigo)
        {
            if (string.IsNullOrEmpty(codigo))
            {
                throw new ArgumentNullException("O código não pode ser nulo ou vazio.", nameof(codigo));
            }

            var response = await _httpClient.GetAsync($"/api/departamento/{codigo}");
            if(!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Erro ao obter departamento: {response.ReasonPhrase}");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<DepartamentoDTO>(jsonString, _serializerOptions);
        }
    }
}
