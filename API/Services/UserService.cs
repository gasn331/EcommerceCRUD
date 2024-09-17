using API.Data;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly MySqlDataAccess _dataAccess;

        public UserService(MySqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<bool> CreateUserAsync(string email, string password) 
        {

            return await _dataAccess.CreateUserAsync(email, password);
        }

        public async Task<bool> ValidateUserAsync(string email, string password) 
        {
            var storedPassword = await _dataAccess.GetPasswordHash(email);

            if(password == null || storedPassword == null )
            {
                return false;
            }

            return password.Equals(storedPassword);
        }
    }
}
