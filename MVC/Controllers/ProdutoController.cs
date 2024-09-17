using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;
using MVC.Services.Interfaces;
using Shared.DTOs;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class ProdutoController : BaseController
    {
        private readonly IProdutoService _produtoService;
        private readonly IDepartamentoService _departamentoService;

        public ProdutoController(IProdutoService produtoService, IDepartamentoService departamentoService)
        {
            _produtoService = produtoService;
            _departamentoService = departamentoService;
        }

        //GET: /Produto
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var totalItems = await _produtoService.GetTotalCountAsync(); // Método para obter o total de produtos, a solução ideal é utilizar cache, por motivos de brevidade fui pelo caminho mais simples.
            // Obter produtos com paginação da API
            var produtos = await _produtoService.GetProdutosAsync(pageNumber, pageSize);

            var viewModel = new ProdutoListViewModel
            {
                Produtos = produtos,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };

            return View(viewModel);
        }

        // GET: /Produto/Details/{codigo}
        public async Task<IActionResult> Details(string codigo) 
        {
            var produto = await _produtoService.GetProdutoAsync(codigo);
            
            if(produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }
        
        // GET: /Produto/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Consultar a lista de departamentos
            var departamentos = await _departamentoService.GetDepartamentosAsync();

            // Criar o modelo de ProdutoCreateViewModel e passar a lista de departamentos
            var viewModel = new ProdutoCreateViewModel
            {
                DepartamentosDisponiveis = departamentos
            };

            return View(viewModel);
        }

        // POST: /Produto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Recuperar o departamento baseado no ID selecionado
                var departamento = await _departamentoService.GetDepartamentoAsync(viewModel.DepartamentoSelecionadoCodigo);

                // Preencher o ProdutoDTO com o departamento selecionado
                viewModel.Produto.Departamento = departamento;

                // Criar o produto
                await _produtoService.CreateProdutoAsync(viewModel.Produto);

                return RedirectToAction(nameof(Index));
            }

            // Recarregar a lista de departamentos em caso de erro
            viewModel.DepartamentosDisponiveis = await _departamentoService.GetDepartamentosAsync();
            return View(viewModel);
        }

        // GET: /Produto/Edit/{codigo}
        [HttpGet]
        public async Task<IActionResult> Edit(string codigo)
        {
            var produto = await _produtoService.GetProdutoAsync(codigo);
            var departamentos = await _departamentoService.GetDepartamentosAsync();

            if (produto == null)
            {
                return NotFound();
            }

            var viewModel = new ProdutoCreateViewModel
            {
                Produto = produto,
                DepartamentosDisponiveis = departamentos,
                DepartamentoSelecionadoCodigo = produto.Departamento?.Codigo // Utilizando operador null conditional
            };

            return View(viewModel);
        }

        // POST: /Produto/Edit/{codigo}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string codigo, ProdutoCreateViewModel viewModel)
        {
            if (codigo != viewModel.Produto.Codigo)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                if (viewModel.DepartamentoSelecionadoCodigo != viewModel.Produto.Departamento?.Codigo)
                {
                    // Recuperar o departamento baseado no ID selecionado
                    var departamento = await _departamentoService.GetDepartamentoAsync(viewModel.DepartamentoSelecionadoCodigo);

                    // Preencher o ProdutoDTO com o departamento selecionado
                    viewModel.Produto.Departamento = departamento;
                }

                await _produtoService.UpdateProdutoAsync(codigo, viewModel.Produto);
                return RedirectToAction(nameof(Index));
            }

            // Recarregar a lista de departamentos em caso de erro
            viewModel.DepartamentosDisponiveis = await _departamentoService.GetDepartamentosAsync();
            return View(viewModel);
        }

        // GET: /Produto/Delete/{codigo}
        public async Task<IActionResult> Delete(string codigo)
        {
            var produto = await _produtoService.GetProdutoAsync(codigo);

            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: /Produto/DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string codigo)
        {
            if (string.IsNullOrEmpty(codigo))
            {
                return BadRequest();
            }

            await _produtoService.DeleteProdutoAsync(codigo);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Search(string codigo)
        {
            // Obter o produto por código
            var produto = await _produtoService.GetProdutoAsync(codigo);

            if (produto == null)
            {
                return NotFound();
            }

            var viewModel = new ProdutoListViewModel
            {
                Produtos = new List<ProdutoDTO> { produto },
                CurrentPage = 1,
                TotalPages = 1,
                PageSize = 1
            };

            return View("Index", viewModel);
        }

    }
}
