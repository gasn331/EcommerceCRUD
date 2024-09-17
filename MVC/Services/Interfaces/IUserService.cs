using Microsoft.AspNetCore.Identity.Data;
using Shared.DTOs;

public interface IUserService
{
    Task<bool> CreateUserAsync(RegisterRequest request);
    Task<bool> ValidateUserAsync(LoginRequest request);
}