using DPoll.Application.Features.Slides;
using MediatR;

namespace DPoll.Application.Features.Slides.Commands.UpdateSlide;
public class UpdateSlideHandler(
    ISlidesRepository SlidesRepository) : IRequestHandler<UpdateSlideCommand, bool>
{
    public async Task<bool> Handle(UpdateSlideCommand request, CancellationToken cancellationToken)
    {
        return await SlidesRepository
            .UpdateSlide(request.Id, request.Type, request.IsVisible, request.Content, cancellationToken);
    }
}
