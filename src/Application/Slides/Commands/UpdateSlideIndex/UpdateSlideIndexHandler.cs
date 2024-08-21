namespace Application.Slides.Commands.UpdateSlideIndex;

using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class UpdateSlideIndexHandler(
    ISlidesRepository SlidesRepository) : IRequestHandler<UpdateSlideIndexCommand, bool>
{
    public async Task<bool> Handle(UpdateSlideIndexCommand request, CancellationToken cancellationToken)
    {
        return await SlidesRepository
            .UpdateSlideIndex(request.Id, request.Index, cancellationToken);
    }
}
