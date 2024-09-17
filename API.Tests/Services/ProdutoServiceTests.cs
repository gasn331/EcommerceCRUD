using API.Data;
using API.Models;
using API.Services;
using AutoMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Tests.Services
{
    [TestFixture]
    public class ProdutoServiceTests
    {
        private Mock<IMySqlDataAccess> _mockDataAccess;
        private Mock<IMapper> _mockMapper;
        private ProdutoService _service;

        [SetUp]
        public void SetUp()
        {
            _mockDataAccess = new Mock<IMySqlDataAccess>();
            _mockMapper = new Mock<IMapper>();
            _service = new ProdutoService(_mockDataAccess.Object, _mockMapper.Object);
        }

        [Test]
        public async Task CreateProdutoAsync_ValidProduto_ShouldReturnProduto()
        {
            // Arrange
            var produto = new Produto { Codigo = "123", Descricao = "Produto A" };
            var produtoId = Guid.NewGuid().ToString(); // Simula o ID gerado pelo banco

            _mockDataAccess
                .Setup(d => d.CreateProdutoAsync(produto))
                .ReturnsAsync(produtoId);

            // Act
            var result = await _service.CreateProdutoAsync(produto);

            // ClassicAssert
            ClassicAssert.IsTrue(Guid.TryParse(result.Id.ToString(), out _), "O ID deve ser um GUID válido.");
            ClassicAssert.AreEqual(produto.Codigo, result.Codigo);
            ClassicAssert.AreEqual(produto.Descricao, result.Descricao);
        }

        [Test]
        public async Task DeleteProdutoAsync_ValidCodigo_ShouldReturnTrue()
        {
            // Arrange
            var codigo = "123";
            _mockDataAccess
                .Setup(d => d.DeleteProdutoAsync(codigo))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteProdutoAsync(codigo);

            // ClassicAssert
            ClassicAssert.IsTrue(result);
        }

        [Test]
        public async Task GetAllProdutosAsync_ProdutosExist_ShouldReturnListOfProdutos()
        {
            // Arrange
            var produtos = new List<Produto>
            {
                new Produto { Codigo = "123", Descricao = "Produto A", Departamento = new Departamento { Id = Guid.Parse("207c3d60-721b-11ef-b151-08bfb88cefed"), Codigo = "020", Descricao = "CONGELADOS" }, Excluido = false },
                new Produto { Codigo = "456", Descricao = "Produto B", Departamento = new Departamento { Id = Guid.Parse("207c3d60-721b-11ef-b151-08bfb88cefed"), Codigo = "020", Descricao = "CONGELADOS" }, Excluido = false }
            };

            var produtoDtos = new List<ProdutoDTO>
            {
                new ProdutoDTO { Codigo = "123", Descricao = "Produto A", Departamento = new DepartamentoDTO { Id = Guid.Parse("207c3d60-721b-11ef-b151-08bfb88cefed"), Codigo = "020", Descricao = "CONGELADOS" }, Excluido = false },
                new ProdutoDTO { Codigo = "456", Descricao = "Produto B", Departamento = new DepartamentoDTO { Id = Guid.Parse("207c3d60-721b-11ef-b151-08bfb88cefed"), Codigo = "020", Descricao = "CONGELADOS" }, Excluido = false }
            };

            _mockDataAccess
                .Setup(d => d.GetProdutosAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(produtos);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<ProdutoDTO>>(It.IsAny<IEnumerable<Produto>>()))
                .Returns(produtoDtos);

            // Act
            var result = await _service.GetAllProdutosAsync(1, 10);

            // ClassicAssert
            ClassicAssert.AreEqual(produtoDtos.Count, result.Count());
        }

        [Test]
        public async Task GetProdutoAsync_ProdutoExists_ShouldReturnProdutoDTO()
        {
            // Arrange
            var codigo = "123";
            var produto = new Produto { Codigo = codigo, Descricao = "Produto A", Departamento = new Departamento { Id = Guid.Parse("207c3d60-721b-11ef-b151-08bfb88cefed"), Codigo = "020", Descricao = "CONGELADOS" }, Excluido = false };
            var produtoDto = new ProdutoDTO { Codigo = codigo, Descricao = "Produto A", Departamento = new DepartamentoDTO { Id = Guid.Parse("207c3d60-721b-11ef-b151-08bfb88cefed"), Codigo = "020", Descricao = "CONGELADOS" }, Excluido = false };

            _mockDataAccess
                .Setup(d => d.GetProdutoAsync(codigo))
                .ReturnsAsync(produto);

            _mockMapper
                .Setup(m => m.Map<ProdutoDTO>(produto))
                .Returns(produtoDto);

            // Act
            var result = await _service.GetProdutoAsync(codigo);

            // ClassicAssert
            ClassicAssert.AreEqual(produtoDto.Codigo, result.Codigo);
            ClassicAssert.AreEqual(produtoDto.Descricao, result.Descricao);
        }

        [Test]
        public async Task GetTotalCountAsync_ShouldReturnTotalCount()
        {
            // Arrange
            var totalCount = 100;
            _mockDataAccess
                .Setup(d => d.GetTotalCountAsync())
                .ReturnsAsync(totalCount);

            // Act
            var result = await _service.GetTotalCountAsync();

            // ClassicAssert
            ClassicAssert.AreEqual(totalCount, result);
        }

        [Test]
        public async Task UpdateProdutoAsync_ValidProduto_ShouldReturnTrue()
        {
            // Arrange
            var produto = new Produto { Codigo = "123", Descricao = "Produto A", Departamento = new Departamento { Id = Guid.Parse("207c3d60-721b-11ef-b151-08bfb88cefed"), Codigo = "020", Descricao = "CONGELADOS" }, Excluido = false };
            _mockDataAccess
                .Setup(d => d.UpdateProdutoAsync(produto))
                .ReturnsAsync(true);

            // Act
            var result = await _service.UpdateProdutoAsync(produto);

            // ClassicAssert
            ClassicAssert.IsTrue(result);
        }

        [Test]
        public async Task CreateProdutoAsync_FailedCreation_ShouldThrowException()
        {
            // Arrange
            var produto = new Produto { Codigo = "123", Descricao = "Produto A", Departamento = new Departamento { Id = Guid.Parse("207c3d60-721b-11ef-b151-08bfb88cefed"), Codigo = "020", Descricao = "CONGELADOS" }, Excluido = false };

            _mockDataAccess
                .Setup(d => d.CreateProdutoAsync(produto))
                .ThrowsAsync(new Exception("Falha na criação"));

            // Act & ClassicAssert
            var ex = ClassicAssert.ThrowsAsync<Exception>(async () => await _service.CreateProdutoAsync(produto));
            ClassicAssert.AreEqual("Falha na criação", ex.Message);
        }
    }
}
