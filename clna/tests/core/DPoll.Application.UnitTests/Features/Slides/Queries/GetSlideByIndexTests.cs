using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Entities;
using DPoll.Application.Features.Slides;
using DPoll.Application.Features.Slides.Queries;
using DPoll.Domain.Common.Errors;
using FluentAssertions;
using Moq;

namespace DPoll.Application.UnitTests.Features.Slides.Queries;
public class GetSlideByIndexTests
{
    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenSlideExists()
    {
        // Arrange
        var presentationId = Guid.NewGuid();
        var slideIndex = 1;
        var slide = new Slide { PresentationId = presentationId, Index = slideIndex };

        var slidesRepositoryMock = new Mock<ISlidesRepository>();
        _ = slidesRepositoryMock.Setup(r => r.GetSlideByIndex(presentationId, slideIndex, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(slide);

        var handler = new GetSlideByIndexHandler(slidesRepositoryMock.Object);
        var query = new GetSlideByIndexQuery { PresentationId = presentationId, Index = slideIndex };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.IsSuccess.Should().BeTrue();
        _ = result.Value.Should().Be(slide);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenSlideDoesNotExist()
    {
        // Arrange
        var presentationId = Guid.NewGuid();
        var slideIndex = 1;

        var slidesRepositoryMock = new Mock<ISlidesRepository>();
        _ = slidesRepositoryMock.Setup(r => r.GetSlideByIndex(presentationId, slideIndex, It.IsAny<CancellationToken>()))
                                .ReturnsAsync((Slide)null);

        var handler = new GetSlideByIndexHandler(slidesRepositoryMock.Object);
        var query = new GetSlideByIndexQuery { PresentationId = presentationId, Index = slideIndex };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.IsSuccess.Should().BeFalse();
        _ = result.Error.Should().BeEquivalentTo(Error.NotFound(EntityType.Slide));
    }
}
