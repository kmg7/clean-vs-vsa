using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Entities;
using DPoll.Application.Features.Presentations;
using DPoll.Application.Features.Slides;
using DPoll.Application.Features.Slides.Commands;
using DPoll.Domain.Common.Errors;
using FluentAssertions;
using Moq;
using System.Text.Json;

namespace DPoll.Application.UnitTests.Features.Slides.Commands;
public class CreateSlideTests
{
    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenPresentationExists()
    {
        // Arrange
        var presentationId = Guid.NewGuid();
        var slideType = "TestType";
        var slideContent = JsonDocument.Parse("{}");
        var slide = new Slide { PresentationId = presentationId, Type = slideType, Content = slideContent };

        var presentationsRepositoryMock = new Mock<IPresentationsRepository>();
        _ = presentationsRepositoryMock.Setup(r => r.PresentationExists(presentationId, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(true);

        var slidesRepositoryMock = new Mock<ISlidesRepository>();
        _ = slidesRepositoryMock.Setup(r => r.GetLastSlideIndex(presentationId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(0);
        _ = slidesRepositoryMock.Setup(r => r.CreateSlide(It.IsAny<Slide>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(slide);

        var handler = new CreateSlideHandler(presentationsRepositoryMock.Object, slidesRepositoryMock.Object);
        var command = new CreateSlideCommand { PresentationId = presentationId, Type = slideType, Content = slideContent };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.IsSuccess.Should().BeTrue();
        _ = result.Value.Should().Be(slide);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenPresentationDoesNotExist()
    {
        // Arrange
        var presentationId = Guid.NewGuid();
        var slideType = "TestType";
        var slideContent = JsonDocument.Parse("{}");

        var presentationsRepositoryMock = new Mock<IPresentationsRepository>();
        _ = presentationsRepositoryMock.Setup(r => r.PresentationExists(presentationId, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(false);

        var slidesRepositoryMock = new Mock<ISlidesRepository>();

        var handler = new CreateSlideHandler(presentationsRepositoryMock.Object, slidesRepositoryMock.Object);
        var command = new CreateSlideCommand { PresentationId = presentationId, Type = slideType, Content = slideContent };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo(Error.NotFound(EntityType.Presentation));
    }

}
