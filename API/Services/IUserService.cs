namespace API.Services
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(string email, string password);
        Task<bool> ValidateUserAsync(string email, string password);
    }
}