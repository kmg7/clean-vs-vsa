using Dpoll.Domain.Entities;
using DPoll.Application.Features.Users;
using DPoll.Application.Features.Users.Queries;
using FluentAssertions;
using Moq;

namespace DPoll.Application.UnitTests.Features.Users.Queries;
public class GetUsersHandlerTests
{
    private readonly Mock<IUsersRepository> _mockRepository;
    private readonly GetUsersHandler _handler;

    public GetUsersHandlerTests()
    {
        _mockRepository = new Mock<IUsersRepository>();
        _handler = new GetUsersHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenUsersExist()
    {
        // Arrange
        var users = new List<User> {
                        new() {
                            Id = Guid.NewGuid(),
                            ClerkId = "clerk1",
                            Email = "john@doe.com",
                            IsActive = true,
                            Username = "john.doe",
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        },
                        new() {
                            Id = Guid.NewGuid(),
                            ClerkId = "clerk2",
                            Email = "jane@doe.com",
                            IsActive = true,
                            Username = "jane.doe",
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        }
                    };
        _ = _mockRepository.Setup(repo => repo.GetUsers(It.IsAny<CancellationToken>())).ReturnsAsync(users);
        var query = new GetUsersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _ = result.IsSuccess.Should().BeTrue();
        _ = result.Value.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenNoUsersExist()
    {
        // Arrange
        var users = new List<User>();
        _ = _mockRepository.Setup(repo => repo.GetUsers(It.IsAny<CancellationToken>())).ReturnsAsync(users);

        var query = new GetUsersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _ = result.IsSuccess.Should().BeTrue();
        _ = result.Value.Should().BeEmpty();
    }
}
