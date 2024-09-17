using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using API.Services;
using API.Data;
using NUnit.Framework.Legacy;

namespace API.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IMySqlDataAccess> _mockDataAccess;
        private UserService _service;

        [SetUp]
        public void SetUp()
        {
            _mockDataAccess = new Mock<IMySqlDataAccess>();
            _service = new UserService(_mockDataAccess.Object);
        }

        [Test]
        public async Task CreateUserAsync_ValidUser_ShouldReturnTrue()
        {
            // Arrange
            var email = "test@exemplo.com";
            var password = "password123";

            _mockDataAccess
                .Setup(d => d.CreateUserAsync(email, password))
                .ReturnsAsync(true);

            // Act
            var result = await _service.CreateUserAsync(email, password);

            // ClassicAssert
            ClassicAssert.IsTrue(result);
        }

        [Test]
        public async Task CreateUserAsync_UserCreationFails_ShouldReturnFalse()
        {
            // Arrange
            var email = "test@exemplo.com";
            var password = "password123";

            _mockDataAccess
                .Setup(d => d.CreateUserAsync(email, password))
                .ReturnsAsync(false);

            // Act
            var result = await _service.CreateUserAsync(email, password);

            // ClassicAssert
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public async Task ValidateUserAsync_ValidEmailAndPassword_ShouldReturnTrue()
        {
            // Arrange
            var email = "test@exemplo.com";
            var password = "password123";
            var storedPassword = "password123";

            _mockDataAccess
                .Setup(d => d.GetPasswordHash(email))
                .ReturnsAsync(storedPassword);

            // Act
            var result = await _service.ValidateUserAsync(email, password);

            // ClassicAssert
            ClassicAssert.IsTrue(result);
        }

        [Test]
        public async Task ValidateUserAsync_InvalidPassword_ShouldReturnFalse()
        {
            // Arrange
            var email = "test@exemplo.com";
            var password = "wrongpassword";
            var storedPassword = "password123";

            _mockDataAccess
                .Setup(d => d.GetPasswordHash(email))
                .ReturnsAsync(storedPassword);

            // Act
            var result = await _service.ValidateUserAsync(email, password);

            // ClassicAssert
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public async Task ValidateUserAsync_NullPassword_ShouldReturnFalse()
        {
            // Arrange
            var email = "test@exemplo.com";
            string password = null;
            var storedPassword = "password123";

            _mockDataAccess
                .Setup(d => d.GetPasswordHash(email))
                .ReturnsAsync(storedPassword);

            // Act
            var result = await _service.ValidateUserAsync(email, password);

            // ClassicAssert
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public async Task ValidateUserAsync_NullStoredPassword_ShouldReturnFalse()
        {
            // Arrange
            var email = "test@exemplo.com";
            var password = "password123";
            string storedPassword = null;

            _mockDataAccess
                .Setup(d => d.GetPasswordHash(email))
                .ReturnsAsync(storedPassword);

            // Act
            var result = await _service.ValidateUserAsync(email, password);

            // ClassicAssert
            ClassicAssert.IsFalse(result);
        }
    }
}
