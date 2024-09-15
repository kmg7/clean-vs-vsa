using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Entities;
using DPoll.Application.Features.Presentations;
using DPoll.Application.Features.Presentations.Queries;
using DPoll.Domain.Common.Errors;
using FluentAssertions;
using Moq;

namespace DPoll.Application.UnitTests.Features.Presentations.Queries;

public class GetPresentationByIdTests
{
    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenPresentationExists()
    {
        // Arrange
        var presentationId = Guid.NewGuid();
        var presentation = new Presentation { Id = presentationId, Title = "Test Presentation" };

        var presentationsRepositoryMock = new Mock<IPresentationsRepository>();
        _ = presentationsRepositoryMock.Setup(r => r.GetPresentationById(presentationId, It.IsAny<CancellationToken>()))
                                       .ReturnsAsync(presentation);

        var handler = new GetPresentationByIdHandler(presentationsRepositoryMock.Object);
        var query = new GetPresentationByIdQuery { Id = presentationId };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.IsSuccess.Should().BeTrue();
        _ = result.Value.Should().Be(presentation);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenPresentationDoesNotExist()
    {
        // Arrange
        var presentationId = Guid.NewGuid();

        var presentationsRepositoryMock = new Mock<IPresentationsRepository>();
        _ = presentationsRepositoryMock.Setup(r => r.GetPresentationById(presentationId, It.IsAny<CancellationToken>()))
                                       .ReturnsAsync((Presentation)null);

        var handler = new GetPresentationByIdHandler(presentationsRepositoryMock.Object);
        var query = new GetPresentationByIdQuery { Id = presentationId };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.IsSuccess.Should().BeFalse();
        _ = result.Error.Should().BeEquivalentTo(Error.NotFound(EntityType.Presentation));
    }
}


