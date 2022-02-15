using Brugner.API.Core.Contracts.Repositories;
using Brugner.API.Core.Contracts.Services;
using Brugner.API.Core.Exceptions;
using Brugner.API.Core.Models.DTOs.Auth;
using Brugner.API.Core.Models.Entities;
using Brugner.API.Core.Options;
using Brugner.API.Core.Services;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Brugner.Tests.Core.Services
{
    public class AuthServiceTests
    {
        private readonly AuthService _authService;
        private readonly Mock<IUsersRepository> _mockUsersRepository;
        private readonly Mock<IHashService> _mockHashService;
        private readonly Mock<IOptionsMonitor<JwtOptions>> _mockJwtOptions;

        public AuthServiceTests()
        {
            _mockUsersRepository = new Mock<IUsersRepository>();
            _mockHashService = new Mock<IHashService>();
            _mockJwtOptions = new Mock<IOptionsMonitor<JwtOptions>>();
            _mockJwtOptions.Setup(x => x.CurrentValue).Returns(new JwtOptions { Secret = "This is not a secret" });

            _authService = new AuthService(_mockUsersRepository.Object, _mockHashService.Object, _mockJwtOptions.Object);
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ValidLogin()
        {
            // Arrange
            var userForAuth = new UserForAuthDTO("email@email.com", "123456");
            var user = GetUser();

            _mockUsersRepository.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _mockHashService.Setup(x => x.ValidatePassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            // Act
            var result = await _authService.LoginAsync(userForAuth);

            // Assert
            Assert.NotNull(result.AccessToken);
            Assert.NotEmpty(result.AccessToken);
        }

        [Fact]
        public async Task LoginAsync_InvalidCredentials_ThrowsBadRequestException()
        {
            // Arrange
            var userForAuth = new UserForAuthDTO("email@email.com", "123456");
            var user = GetUser();

            _mockUsersRepository.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _mockHashService.Setup(x => x.ValidatePassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidArgumentAPIException>(async () => await _authService.LoginAsync(userForAuth));
        }

        [Fact]
        public async Task ChangePasswordAsync_ValidCredentials_PasswordChanged()
        {
            // Arrange
            var changePassword = new ChangePasswordDTO { CurrentPassword = "1234", NewPassword = "abcd", RepeatNewPassword = "abcd" };
            var user = GetUser();

            _mockUsersRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                 .ReturnsAsync(user);

            _mockHashService.Setup(x => x.ValidatePassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            _mockHashService.Setup(x => x.HashPassword(It.IsAny<string>()))
                .Returns("jklsdanmxchjkrt9324");

            // Act
            await _authService.ChangePasswordAsync(changePassword, 1);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public async Task ChangePasswordAsync_InvalidCredentials_ThrowBadRequestException()
        {
            // Arrange
            var changePassword = new ChangePasswordDTO { CurrentPassword = "1234", NewPassword = "abcd", RepeatNewPassword = "abcd" };
            var user = GetUser();

            _mockUsersRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                 .ReturnsAsync(user);

            _mockHashService.Setup(x => x.ValidatePassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            _mockHashService.Setup(x => x.HashPassword(It.IsAny<string>()))
                .Returns("jklsdanmxchjkrt9324");

            // Act & Assert
            await Assert.ThrowsAsync<InvalidArgumentAPIException>(async () => await _authService.ChangePasswordAsync(changePassword, 1));
        }

        private static User GetUser()
        {
            return new User
            {
                Email = "email@email.com",
                CreatedAt = DateTime.UtcNow,
                FirstName = "Dr",
                LastName = "D",
                Id = 1,
                PasswordHash = "92384kjd0as9d9aksjdk",
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}
