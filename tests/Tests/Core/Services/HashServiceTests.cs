using Brugner.API.Core.Services;
using System;
using Xunit;

namespace Brugner.Tests.Core.Services
{
    public class HashServiceTests
    {
        private readonly HashService _hashService;

        public HashServiceTests()
        {
            _hashService = new HashService();
        }

        [Fact]
        public void HashPassword_ValidString_PasswordHash()
        {
            // Act
            var result = _hashService.HashPassword("asd");

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void HashPassword_EmptyString_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _hashService.HashPassword(string.Empty));
        }

        [Fact]
        public void ValidatePassword_PlanAndHashMatch_True()
        {
            // Arrange
            string plainPassword = "asd";
            string hashedPassword = "1000:c3GiveyspmMozxzV6xBXXUZWclkkZWsX:UyPFLD/GSEpdPj8nWWZOjTyGMcw=";

            // Act
            var result = _hashService.ValidatePassword(plainPassword, hashedPassword);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidatePassword_PlanAndHashDontMatch_False()
        {
            // Arrange
            string plainPassword = "asd";
            string hashedPassword = "1000:c3GiveyspmMozxzV6xBXXUZWclkkZWsX:UyPFLD/GSEpdPj8nWWZOjTyGfck=";

            // Act
            var result = _hashService.ValidatePassword(plainPassword, hashedPassword);

            // Assert
            Assert.False(result);
        }
    }
}
