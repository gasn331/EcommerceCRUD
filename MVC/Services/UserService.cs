using MVC.Services.Interfaces;
using Shared.DTOs;
using System.Text;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> CreateUserAsync(RegisterRequest request)
    {
        // Codificar a senha em Base64
        var encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Password));
        var encodedRequest = new RegisterRequest
        {
            Email = request.Email,
            Password = encodedPassword
        };

        var response = await _httpClient.PostAsJsonAsync("/api/user/register", encodedRequest);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ValidateUserAsync(LoginRequest request)
    {
        // Codificar a senha em Base64
        var encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Password));
        var encodedRequest = new LoginRequest
        {
            Email = request.Email,
            Password = encodedPassword
        };

        var response = await _httpClient.PostAsJsonAsync("/api/user/login", encodedRequest);
        return response.IsSuccessStatusCode;
    }
}
