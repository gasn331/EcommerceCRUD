using Moq;
using NUnit.Framework;
using API.Controllers;
using API.Services;
using Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using NUnit.Framework.Legacy;

namespace API.Tests.Controllers
{
    [TestFixture]
    public class DepartamentoControllerTests
    {
        private Mock<IDepartamentoService> _mockDepartamentoService;
        private DepartamentoController _controller;

        [SetUp]
        public void Setup()
        {
            _mockDepartamentoService = new Mock<IDepartamentoService>();
            _controller = new DepartamentoController(_mockDepartamentoService.Object);
        }

        [Test]
        public async Task GetDepartamentos_ShouldReturnOkResult_WithListOfDepartamentos()
        {
            // Arrange
            var mockDepartamentos = new List<DepartamentoDTO>
            {
                new DepartamentoDTO { Codigo = "001", Descricao = "Financeiro" },
                new DepartamentoDTO { Codigo = "002", Descricao = "TI" }
            };

            _mockDepartamentoService
                .Setup(service => service.GetAllDepartamentosAsync())
                .ReturnsAsync(mockDepartamentos);

            // Act
            var result = await _controller.GetDepartamentos();

            // Assert
            var actionResult = result as ActionResult<IEnumerable<DepartamentoDTO>>;
            var okResult = actionResult.Result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual(200, okResult.StatusCode);

            var departamentos = okResult.Value as IEnumerable<DepartamentoDTO>;
            ClassicAssert.IsNotNull(departamentos);
            ClassicAssert.AreEqual(2, departamentos.Count());
        }

        [Test]
        public async Task GetDepartamento_ValidCodigo_ShouldReturnOkResult_WithDepartamento()
        {
            // Arrange
            var mockDepartamento = new DepartamentoDTO { Codigo = "001", Descricao = "Financeiro" };

            _mockDepartamentoService
                .Setup(service => service.GetDepartamentoAsync("001"))
                .ReturnsAsync(mockDepartamento);

            // Act
            var result = await _controller.GetDepartamento("001");

            // Assert
            var actionResult = result as ActionResult<DepartamentoDTO>;
            var okResult = actionResult.Result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual(200, okResult.StatusCode);

            var departamento = okResult.Value as DepartamentoDTO;
            ClassicAssert.IsNotNull(departamento);
            ClassicAssert.AreEqual("001", departamento.Codigo);
            ClassicAssert.AreEqual("Financeiro", departamento.Descricao);
        }

        [Test]
        public async Task GetDepartamento_InvalidCodigo_ShouldReturnNotFound()
        {
            // Arrange
            _mockDepartamentoService
                .Setup(service => service.GetDepartamentoAsync("999"))
                .ReturnsAsync((DepartamentoDTO)null);

            // Act
            var result = await _controller.GetDepartamento("999");

            // Assert
            var actionResult = result as ActionResult<DepartamentoDTO>;
            ClassicAssert.IsInstanceOf<NotFoundResult>(actionResult.Result);
        }
    }
}
