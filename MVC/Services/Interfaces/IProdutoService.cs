using Shared.DTOs;

public interface IProdutoService
{
    Task<IEnumerable<ProdutoDTO>> GetProdutosAsync(int pageNumber = 1, int pageSize = 10);
    Task<ProdutoDTO> GetProdutoAsync(string codigo);
    Task<ProdutoDTO> CreateProdutoAsync(ProdutoDTO produtoDto);
    Task<bool> UpdateProdutoAsync(string codigo, ProdutoDTO produtoDto);
    Task<bool> DeleteProdutoAsync(string codigo);
    Task<int> GetTotalCountAsync();
}