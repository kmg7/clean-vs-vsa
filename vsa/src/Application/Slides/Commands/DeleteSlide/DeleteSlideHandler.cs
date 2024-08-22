namespace Application.Slides.Commands.DeleteSlide;

using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class DeleteSlideHandler(ISlidesRepository SlidesRepository)
    : IRequestHandler<DeleteSlideCommand, bool>
{
    public async Task<bool> Handle(DeleteSlideCommand request, CancellationToken cancellationToken)
    {
        return await SlidesRepository
            .DeleteSlide(request.Id, cancellationToken);
    }
}
