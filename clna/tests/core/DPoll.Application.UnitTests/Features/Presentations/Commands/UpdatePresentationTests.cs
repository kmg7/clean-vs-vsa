using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Entities;
using DPoll.Application.Features.Presentations;
using DPoll.Application.Features.Presentations.Commands;
using DPoll.Application.Features.Users;
using DPoll.Domain.Common.Errors;
using FluentAssertions;
using Moq;

namespace DPoll.Application.UnitTests.Features.Presentations.Commands;

public class UpdatePresentationTests
{
    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenPresentationAndUserExist()
    {
        // Arrange
        var presentationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var newTitle = "New Title";
        var presentation = new Presentation { Id = presentationId, Title = "Old Title" };

        var presentationsRepositoryMock = new Mock<IPresentationsRepository>();
        _ = presentationsRepositoryMock.Setup(r => r.GetPresentationById(presentationId, It.IsAny<CancellationToken>()))
                                       .ReturnsAsync(presentation);
        _ = presentationsRepositoryMock.Setup(r => r.UpdatePresentation(presentation, It.IsAny<CancellationToken>()))
                                       .ReturnsAsync(true);

        var usersRepositoryMock = new Mock<IUsersRepository>();
        _ = usersRepositoryMock.Setup(r => r.UserExists(userId, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(true);

        var handler = new UpdatePresentationHandler(usersRepositoryMock.Object, presentationsRepositoryMock.Object);
        var command = new UpdatePresentationCommand { Id = presentationId, UserId = userId, Title = newTitle };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.IsSuccess.Should().BeTrue();
        _ = result.Value.Should().BeTrue();
        _ = presentation.Title.Should().Be(newTitle);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenPresentationDoesNotExist()
    {
        // Arrange
        var presentationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var newTitle = "New Title";

        var presentationsRepositoryMock = new Mock<IPresentationsRepository>();
        _ = presentationsRepositoryMock.Setup(r => r.GetPresentationById(presentationId, It.IsAny<CancellationToken>()))
                                       .ReturnsAsync((Presentation)null);

        var usersRepositoryMock = new Mock<IUsersRepository>();

        var handler = new UpdatePresentationHandler(usersRepositoryMock.Object, presentationsRepositoryMock.Object);
        var command = new UpdatePresentationCommand { Id = presentationId, UserId = userId, Title = newTitle };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.IsSuccess.Should().BeFalse();
        _ = result.Error.Should().BeEquivalentTo(Error.NotFound(EntityType.Presentation));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenUserDoesNotExist()
    {
        // Arrange
        var presentationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var newTitle = "New Title";
        var presentation = new Presentation { Id = presentationId, Title = "Old Title" };

        var presentationsRepositoryMock = new Mock<IPresentationsRepository>();
        _ = presentationsRepositoryMock.Setup(r => r.GetPresentationById(presentationId, It.IsAny<CancellationToken>()))
                                       .ReturnsAsync(presentation);

        var usersRepositoryMock = new Mock<IUsersRepository>();
        _ = usersRepositoryMock.Setup(r => r.UserExists(userId, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(false);

        var handler = new UpdatePresentationHandler(usersRepositoryMock.Object, presentationsRepositoryMock.Object);
        var command = new UpdatePresentationCommand { Id = presentationId, UserId = userId, Title = newTitle };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.IsSuccess.Should().BeFalse();
        _ = result.Error.Should().BeEquivalentTo(Error.NotFound(EntityType.User));
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenNoFieldsAreChanged()
    {
        // Arrange
        var presentationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var presentation = new Presentation { Id = presentationId, Title = "Old Title" };

        var presentationsRepositoryMock = new Mock<IPresentationsRepository>();
        _ = presentationsRepositoryMock.Setup(r => r.GetPresentationById(presentationId, It.IsAny<CancellationToken>()))
                                       .ReturnsAsync(presentation);

        var usersRepositoryMock = new Mock<IUsersRepository>();
        _ = usersRepositoryMock.Setup(r => r.UserExists(userId, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(true);

        var handler = new UpdatePresentationHandler(usersRepositoryMock.Object, presentationsRepositoryMock.Object);
        var command = new UpdatePresentationCommand { Id = presentationId, UserId = userId, Title = "Old Title" };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.IsSuccess.Should().BeTrue();
        _ = result.Value.Should().BeTrue();
    }
}

