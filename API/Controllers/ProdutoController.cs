using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Shared.DTOs;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;

        public ProdutoController(IProdutoService produtoService, IMapper mapper)
        {
            _produtoService = produtoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutos(int pageNumber = 1, int pageSize = 10)
        {
            var produtos = await _produtoService.GetAllProdutosAsync(pageNumber, pageSize);

            return Ok(produtos);
        }

        //GET : api/produto/{codigo}
        [HttpGet("{codigo}")]
        public async Task<ActionResult<ProdutoDTO>> GetProduto(string codigo)
        {
            var produto = await _produtoService.GetProdutoAsync(codigo);
            
            return Ok(produto);
        }

        //POST: api/produto
        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> CreateProduto(ProdutoDTO produtoDto)
        {
            var produto = _mapper.Map<Produto>(produtoDto);

            var createdProduto = await _produtoService.CreateProdutoAsync(produto);

            var createdProdutoDto = _mapper.Map<ProdutoDTO>(createdProduto);

            return CreatedAtAction(nameof(GetProduto), new {codigo = createdProdutoDto.Codigo}, createdProduto);
        }

        //PUT: api/produto/{codigo}
        [HttpPut("{codigo}")]
        public async Task<IActionResult> UpdateProduto(string codigo, ProdutoDTO produtoDto) 
        {
            if (codigo != produtoDto.Codigo) 
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDto);

            var result = await _produtoService.UpdateProdutoAsync(produto);

            if (!result)
                return NotFound();

            return NoContent();
        }

        //DELETE: api/produto/{codigo}
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteProduto(string codigo)
        {
            var result = await _produtoService.DeleteProdutoAsync(codigo);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("totalCount")]
        public async Task<ActionResult<int>> GetTotalCount()
        {
            try
            {
                var totalCount = await _produtoService.GetTotalCountAsync();
                return Ok(totalCount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
