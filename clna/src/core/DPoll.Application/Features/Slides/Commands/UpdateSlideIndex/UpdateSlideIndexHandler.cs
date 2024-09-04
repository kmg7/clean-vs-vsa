using MediatR;

namespace DPoll.Application.Features.Slides.Commands.UpdateSlideIndex;
public class UpdateSlideIndexHandler(
    ISlidesRepository SlidesRepository) : IRequestHandler<UpdateSlideIndexCommand, bool>
{
    public async Task<bool> Handle(UpdateSlideIndexCommand request, CancellationToken cancellationToken)
    {
        return await SlidesRepository
            .UpdateSlideIndex(request.Id, request.Index, cancellationToken);
    }
}
