using API.Models;
using MySql.Data.MySqlClient;

namespace API.Data
{
    public interface IMySqlDataAccess
    {
        Task<IEnumerable<Produto>> GetProdutosAsync(int pageNumber, int pageSize);
        Task<Produto> GetProdutoAsync(string codigo);
        Task<string> CreateProdutoAsync(Produto produto);
        Task<bool> DeleteProdutoAsync(string codigo);
        Task<bool> UpdateProdutoAsync(Produto produto);
        Task<IEnumerable<Departamento>> GetDepartamentosAsync();
        Task<Departamento> GetDepartamentoAsync(string codigo);
        Task<int> GetTotalCountAsync();
        Task<bool> CreateUserAsync(string email, string passwordHash);
        Task<string> GetPasswordHash(string email);
    }
}
