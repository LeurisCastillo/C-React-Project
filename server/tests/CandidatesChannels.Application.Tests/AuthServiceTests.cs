using CandidatesChannels.Application.Contracts.Persistence;
using CandidatesChannels.Application.Contracts.Security;
using CandidatesChannels.Application.DTOs.Auth;
using CandidatesChannels.Application.Services;
using CandidatesChannels.Domain.Entities;
using Moq;
using Xunit;

namespace CandidatesChannels.Application.Tests;

public sealed class AuthServiceTests
{
    [Fact]
    public async Task LoginAsync_ReturnsToken_WhenCredentialsAreValid()
    {
        var users = new Mock<IUserRepository>();
        var hasher = new Mock<IPasswordHasher>();
        var jwt = new Mock<IJwtTokenGenerator>();

        var user = new User("admin@demo.com", "HASH", "Admin");

        users.Setup(x => x.GetByEmailAsync("admin@demo.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        hasher.Setup(x => x.Verify("Admin123*", "HASH")).Returns(true);
        jwt.Setup(x => x.GenerateToken("admin@demo.com", "Admin")).Returns("TOKEN");

        var sut = new AuthService(users.Object, hasher.Object, jwt.Object);

        var result = await sut.LoginAsync(new LoginRequest("admin@demo.com", "Admin123*"), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("TOKEN", result!.AccessToken);
        Assert.Equal("Admin", result.Role);
    }
}
