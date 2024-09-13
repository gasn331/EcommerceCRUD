using API.Data;
using API.Models;
using Shared.DTOs;
using AutoMapper;

namespace API.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly MySqlDataAccess _dataAccess;
        private readonly IMapper _mapper;

        public ProdutoService(MySqlDataAccess dataAccess, IMapper mapper) 
        {
            _dataAccess = dataAccess;
            _mapper = mapper;
        }
        public async Task<Produto> CreateProdutoAsync(Produto produto)
        {
            var id = await _dataAccess.CreateProdutoAsync(produto);

            produto.Id = new Guid(id);

            return produto;
        }

        public async Task<bool> DeleteProdutoAsync(string codigo)
        {
            var result = await _dataAccess.DeleteProdutoAsync(codigo);

            return result;
        }

        public async Task<IEnumerable<ProdutoDTO>> GetAllProdutosAsync(int pageNumber, int pageSize)
        {
            var produtos = await _dataAccess.GetProdutosAsync(pageNumber, pageSize);

            if (produtos != null && produtos.ToList().Count > 0)
            {
                return _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            }

            return new List<ProdutoDTO>();
        }

        public async Task<ProdutoDTO> GetProdutoAsync(string codigo)
        {
            var produto = await _dataAccess.GetProdutoAsync(codigo);

            if (produto != null)
                return _mapper.Map<ProdutoDTO>(produto);

            return new ProdutoDTO();
        }

        public async Task<bool> UpdateProdutoAsync(Produto produto)
        {
            var result = await _dataAccess.UpdateProdutoAsync(produto);

            return result;
        }
    }
}
