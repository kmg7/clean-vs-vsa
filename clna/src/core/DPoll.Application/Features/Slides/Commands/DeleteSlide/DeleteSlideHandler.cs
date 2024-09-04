using DPoll.Application.Features.Slides;
using MediatR;

namespace DPoll.Application.Features.Slides.Commands.DeleteSlide;
public class DeleteSlideHandler(ISlidesRepository SlidesRepository)
    : IRequestHandler<DeleteSlideCommand, bool>
{
    public async Task<bool> Handle(DeleteSlideCommand request, CancellationToken cancellationToken)
    {
        return await SlidesRepository
            .DeleteSlide(request.Id, cancellationToken);
    }
}
