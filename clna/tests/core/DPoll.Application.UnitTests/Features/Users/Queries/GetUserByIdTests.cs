using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Entities;
using DPoll.Application.Features.Users;
using DPoll.Application.Features.Users.Queries;
using DPoll.Domain.Common.Errors;
using FluentAssertions;
using Moq;

namespace DPoll.Application.UnitTests.Features.Users.Queries;
public class GetUserByIdTests
{
    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var repositoryMock = new Mock<IUsersRepository>();
        _ = repositoryMock.Setup(r => r.GetUserById(userId, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(user);

        var handler = new GetUserByIdHandler(repositoryMock.Object);
        var query = new GetUserByIdQuery { Id = userId };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.IsSuccess.Should().BeTrue();
        _ = result.Value.Should().Be(user);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var repositoryMock = new Mock<IUsersRepository>();
        _ = repositoryMock.Setup(r => r.GetUserById(userId, It.IsAny<CancellationToken>()))
                      .ReturnsAsync((User)null);

        var handler = new GetUserByIdHandler(repositoryMock.Object);
        var query = new GetUserByIdQuery { Id = userId };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.IsSuccess.Should().BeFalse();
        _ = result.Error.Should().BeEquivalentTo(Error.NotFound(EntityType.User));
    }
}
