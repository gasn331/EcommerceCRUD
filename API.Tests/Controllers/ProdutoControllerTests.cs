using Moq;
using API.Controllers;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using API.Models;
using NUnit.Framework.Legacy;
using System.Linq;
using System;
using Microsoft.AspNetCore.Identity;

namespace API.Tests.Controllers
{
    [TestFixture]
    public class ProdutoControllerTests
    {
        private Mock<IProdutoService> _produtoServiceMock;
        private Mock<IMapper> _mapperMock;
        private ProdutoController _controller;

        [SetUp]
        public void Setup()
        {
            _produtoServiceMock = new Mock<IProdutoService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new ProdutoController(_produtoServiceMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetProdutos_ShouldReturnOk_WithListOfProdutos()
        {
            // Arrange
            var produtos = new List<ProdutoDTO>
            {
                new ProdutoDTO { Codigo = "001", Descricao = "Produto 1" },
                new ProdutoDTO { Codigo = "002", Descricao = "Produto 2" }
            };

            _produtoServiceMock.Setup(service => service.GetAllProdutosAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(produtos);

            // Act
            var result = await _controller.GetProdutos();

            // Assert
            var okResult = result.Result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            var returnValue = okResult.Value as IEnumerable<ProdutoDTO>;
            ClassicAssert.AreEqual(2, returnValue.ToList().Count());
        }

        [Test]
        public async Task GetProduto_ProdutoExiste_DeveRetornarOk()
        {
            // Arrange
            var produtoDto = new ProdutoDTO { Codigo = "123", Descricao = "Produto Teste" };
            _produtoServiceMock.Setup(service => service.GetProdutoAsync("123"))
                .ReturnsAsync(produtoDto);

            // Act
            var result = await _controller.GetProduto("123");

            // Assert
            var okResult = result.Result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            var returnValue = okResult.Value as ProdutoDTO;
            ClassicAssert.AreEqual("Produto Teste", returnValue.Descricao);
        }

        [Test]
        public async Task GetProduto_ProdutoNaoExiste_DeveRetornarNotFound()
        {
            // Arrange
            _produtoServiceMock.Setup(service => service.GetProdutoAsync("9999"))
                .ReturnsAsync((ProdutoDTO)null);

            // Act
            var result = await _controller.GetProduto("9999");

            // Assert
            ClassicAssert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task CreateProduto_ValidProduto_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var produtoDto = new ProdutoDTO { Codigo = "123", Descricao = "Produto Teste", Departamento = new DepartamentoDTO { Id = Guid.Parse("207c3d60-721b-11ef-b151-08bfb88cefed"), Codigo = "020", Descricao = "CONGELADOS" }, Excluido = false  };
            var produto = new Produto { Codigo = "123", Descricao = "Produto Teste", DepartamentoId = Guid.Parse("207c3d60-721b-11ef-b151-08bfb88cefed"), Departamento = new Departamento { Id = Guid.Parse("207c3d60-721b-11ef-b151-08bfb88cefed"), Codigo = "020", Descricao = "CONGELADOS" }, Excluido = false };

            // Configurando o mapeamento entre ProdutoDTO e Produto
            _mapperMock.Setup(m => m.Map<Produto>(produtoDto)).Returns(produto);

            // Configurando a criação do produto no serviço
            _produtoServiceMock.Setup(s => s.CreateProdutoAsync(produto)).ReturnsAsync(produto);

            // Configurando o mapeamento de volta para ProdutoDTO
            _mapperMock.Setup(m => m.Map<ProdutoDTO>(produto)).Returns(produtoDto);

            // Act
            var result = await _controller.CreateProduto(produtoDto);

            // Assert
            ClassicAssert.IsInstanceOf<ActionResult<ProdutoDTO>>(result);

            ClassicAssert.IsInstanceOf<CreatedAtActionResult>(result.Result);

            var createdAtActionResult = result.Result as CreatedAtActionResult;
            ClassicAssert.IsNotNull(createdAtActionResult);
            ClassicAssert.AreEqual("GetProduto", createdAtActionResult.ActionName);

            var returnedProdutoDto = createdAtActionResult.Value as ProdutoDTO;

            ClassicAssert.IsNotNull(returnedProdutoDto);
            ClassicAssert.AreEqual(produtoDto, returnedProdutoDto);
        }

    }
}
