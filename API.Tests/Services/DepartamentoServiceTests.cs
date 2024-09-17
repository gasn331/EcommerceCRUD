using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Services;
using AutoMapper;
using Shared.DTOs;
using API.Models;
using NUnit.Framework.Legacy;

namespace API.Tests.Services
{
    [TestFixture]
    public class DepartamentoServiceTests
    {
        private Mock<IMySqlDataAccess> _mockDataAccess;
        private Mock<IMapper> _mockMapper;
        private DepartamentoService _service;

        [SetUp]
        public void SetUp()
        {
            _mockDataAccess = new Mock<IMySqlDataAccess>();
            _mockMapper = new Mock<IMapper>();
            _service = new DepartamentoService(_mockDataAccess.Object, _mockMapper.Object);
        }

        [Test]
        public async Task GetAllDepartamentosAsync_DepartamentosExist_ShouldReturnListOfDepartamentos()
        {
            // Arrange
            var departamentos = new List<Departamento>
            {
                new Departamento { Codigo = "001", Descricao = "TI" },
                new Departamento { Codigo = "002", Descricao = "RH" }
            };

            var departamentoDtos = new List<DepartamentoDTO>
            {
                new DepartamentoDTO { Codigo = "001", Descricao = "TI" },
                new DepartamentoDTO { Codigo = "002", Descricao = "RH" }
            };

            _mockDataAccess
                .Setup(d => d.GetDepartamentosAsync())
                .ReturnsAsync(departamentos);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<DepartamentoDTO>>(departamentos))
                .Returns(departamentoDtos);

            // Act
            var result = await _service.GetAllDepartamentosAsync();

            // ClassicAssert
            ClassicAssert.AreEqual(departamentoDtos.Count, result.Count());
        }

        [Test]
        public async Task GetAllDepartamentosAsync_NoDepartamentos_ShouldReturnEmptyList()
        {
            // Arrange
            var departamentos = new List<Departamento>();

            _mockDataAccess
                .Setup(d => d.GetDepartamentosAsync())
                .ReturnsAsync(departamentos);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<DepartamentoDTO>>(departamentos))
                .Returns(new List<DepartamentoDTO>());

            // Act
            var result = await _service.GetAllDepartamentosAsync();

            // ClassicAssert
            ClassicAssert.IsEmpty(result);
        }

        [Test]
        public async Task GetDepartamentoAsync_DepartamentoExists_ShouldReturnDepartamentoDTO()
        {
            // Arrange
            var codigo = "001";
            var departamento = new Departamento { Codigo = codigo, Descricao = "TI" };
            var departamentoDto = new DepartamentoDTO { Codigo = codigo, Descricao = "TI" };

            _mockDataAccess
                .Setup(d => d.GetDepartamentoAsync(codigo))
                .ReturnsAsync(departamento);

            _mockMapper
                .Setup(m => m.Map<DepartamentoDTO>(departamento))
                .Returns(departamentoDto);

            // Act
            var result = await _service.GetDepartamentoAsync(codigo);

            // ClassicAssert
            ClassicAssert.AreEqual(departamentoDto.Codigo, result.Codigo);
            ClassicAssert.AreEqual(departamentoDto.Descricao, result.Descricao);
        }

        [Test]
        public async Task GetDepartamentoAsync_DepartamentoNotFound_ShouldReturnEmptyDepartamentoDTO()
        {
            // Arrange
            var codigo = "999";
            Departamento departamento = null;
            var departamentoDto = new DepartamentoDTO();

            _mockDataAccess
                .Setup(d => d.GetDepartamentoAsync(codigo))
                .ReturnsAsync(departamento);

            _mockMapper
                .Setup(m => m.Map<DepartamentoDTO>(departamento))
                .Returns(departamentoDto);

            // Act
            var result = await _service.GetDepartamentoAsync(codigo);

            // ClassicAssert
            ClassicAssert.AreEqual(departamentoDto.Codigo, result.Codigo);
            ClassicAssert.AreEqual(departamentoDto.Descricao, result.Descricao);
        }
    }
}
