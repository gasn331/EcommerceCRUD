using MVC.Services.Interfaces;
using Shared.DTOs;
using System.Text.Json;
using System.Text;

public class ProdutoService : IProdutoService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;

    public ProdutoService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory?.CreateClient("ApiClient") ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            // Adicione outras opções conforme necessário
        };
    }

    public async Task<IEnumerable<ProdutoDTO>> GetProdutosAsync(int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber <= 0 || pageSize <= 0)
        {
            throw new ArgumentOutOfRangeException("pageNumber e pageSize devem ser maiores que zero.");
        }

        var response = await _httpClient.GetAsync($"/api/produto?pageNumber={pageNumber}&pageSize={pageSize}");
        if (!response.IsSuccessStatusCode)
        {
            // Pode ser interessante logar ou lançar uma exceção aqui
            throw new HttpRequestException($"Erro ao obter produtos: {response.ReasonPhrase}");
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<ProdutoDTO>>(jsonString, _serializerOptions) ?? Enumerable.Empty<ProdutoDTO>();
    }

    public async Task<ProdutoDTO> GetProdutoAsync(string codigo)
    {
        if (string.IsNullOrEmpty(codigo))
        {
            throw new ArgumentException("O código não pode ser nulo ou vazio.", nameof(codigo));
        }

        var response = await _httpClient.GetAsync($"/api/produto/{codigo}");
        if (!response.IsSuccessStatusCode)
        {
            // Pode ser interessante logar ou lançar uma exceção aqui
            throw new HttpRequestException($"Erro ao obter produto: {response.ReasonPhrase}");
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ProdutoDTO>(jsonString, _serializerOptions);
    }

    public async Task<ProdutoDTO> CreateProdutoAsync(ProdutoDTO produtoDto)
    {
        if (produtoDto == null)
        {
            throw new ArgumentNullException(nameof(produtoDto));
        }

        var jsonContent = JsonSerializer.Serialize(produtoDto);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/api/produto", content);
        if (!response.IsSuccessStatusCode)
        {
            // Pode ser interessante logar ou lançar uma exceção aqui
            throw new HttpRequestException($"Erro ao criar produto: {response.ReasonPhrase}");
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ProdutoDTO>(jsonString, _serializerOptions);
    }

    public async Task<bool> UpdateProdutoAsync(string codigo, ProdutoDTO produtoDto)
    {
        if (string.IsNullOrEmpty(codigo))
        {
            throw new ArgumentException("O código não pode ser nulo ou vazio.", nameof(codigo));
        }

        if (produtoDto == null)
        {
            throw new ArgumentNullException(nameof(produtoDto));
        }

        var jsonContent = JsonSerializer.Serialize(produtoDto);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"/api/produto/{codigo}", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteProdutoAsync(string codigo)
    {
        if (string.IsNullOrEmpty(codigo))
        {
            throw new ArgumentException("O código não pode ser nulo ou vazio.", nameof(codigo));
        }

        var response = await _httpClient.DeleteAsync($"/api/produto/{codigo}");
        return response.IsSuccessStatusCode;
    }

    public async Task<int> GetTotalCountAsync() 
    {
        var response = await _httpClient.GetAsync("/api/Produto/totalCount");

        if (!response.IsSuccessStatusCode)
        {
            // Loga o erro para análise
            var errorMessage = $"Erro ao obter contagem total de produtos: {response.ReasonPhrase}";
            // Opcional: Log de erro ou lançar uma exceção personalizada
            throw new HttpRequestException(errorMessage);
        }

        // Lê a resposta como string
        var jsonString = await response.Content.ReadAsStringAsync();

        // Desserializa a string JSON para um inteiro
        int totalCount;
        try
        {
            totalCount = JsonSerializer.Deserialize<int>(jsonString, _serializerOptions);
        }
        catch (JsonException ex)
        {
            // Opcional: Loga a exceção de deserialização
            throw new InvalidOperationException("Erro ao desserializar o total count dos produtos.", ex);
        }

        // Retorna o total count
        return totalCount;
    }
}
