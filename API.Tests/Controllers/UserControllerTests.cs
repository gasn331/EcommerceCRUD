using Moq;
using NUnit.Framework;
using API.Controllers;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;
using NUnit.Framework.Legacy;

namespace API.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService> _mockUserService;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Test]
        public async Task Register_ValidRequest_ShouldReturnOkResult()
        {
            // Arrange
            var registerRequest = new RegisterRequest { Email = "test@exemplo.com", Password = "Password123" };

            _mockUserService
                .Setup(service => service.CreateUserAsync(registerRequest.Email, registerRequest.Password))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Register(registerRequest);

            // ClassicAssert
            var okResult = result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual(200, okResult.StatusCode);
            ClassicAssert.AreEqual("Usuário criado com sucesso!", okResult.Value);
        }

        [Test]
        public async Task Register_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var registerRequest = new RegisterRequest { Email = "test@exemplo.com", Password = "Password123" };

            _mockUserService
                .Setup(service => service.CreateUserAsync(registerRequest.Email, registerRequest.Password))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Register(registerRequest);

            // ClassicAssert
            var badRequestResult = result as BadRequestObjectResult;
            ClassicAssert.IsNotNull(badRequestResult);
            ClassicAssert.AreEqual(400, badRequestResult.StatusCode);
            ClassicAssert.AreEqual("Falha na criação do usuário", badRequestResult.Value);
        }

        [Test]
        public async Task Login_ValidRequest_ShouldReturnOkResult()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "test@exemplo.com", Password = "Password123" };

            _mockUserService
                .Setup(service => service.ValidateUserAsync(loginRequest.Email, loginRequest.Password))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Login(loginRequest);

            // ClassicAssert
            var okResult = result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual(200, okResult.StatusCode);
            ClassicAssert.AreEqual("Usuario válido", okResult.Value);
        }

        [Test]
        public async Task Login_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "test@exemplo.com", Password = "WrongPassword" };

            _mockUserService
                .Setup(service => service.ValidateUserAsync(loginRequest.Email, loginRequest.Password))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Login(loginRequest);

            // ClassicAssert
            var badRequestResult = result as BadRequestObjectResult;
            ClassicAssert.IsNotNull(badRequestResult);
            ClassicAssert.AreEqual(400, badRequestResult.StatusCode);
            ClassicAssert.AreEqual("Usuário inválido", badRequestResult.Value);
        }
    }
}
