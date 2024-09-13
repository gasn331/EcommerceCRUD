
using API.Models;
using Shared.DTOs;

namespace API.Services
{
    public interface IProdutoService
    {
        Task<Produto> CreateProdutoAsync(Produto produto);
        Task<bool> DeleteProdutoAsync(string codigo);
        Task<IEnumerable<ProdutoDTO>> GetAllProdutosAsync(int pageSize, int pageNumber);
        Task<ProdutoDTO> GetProdutoAsync(string codigo);
        Task<bool> UpdateProdutoAsync(Produto produto);
    }
}
