namespace Application.Slides.Commands.UpdateSlide;

using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class UpdateSlideHandler(
    ISlidesRepository SlidesRepository) : IRequestHandler<UpdateSlideCommand, bool>
{
    public async Task<bool> Handle(UpdateSlideCommand request, CancellationToken cancellationToken)
    {
        return await SlidesRepository
            .UpdateSlide(request.Id, request.Type, request.IsVisible, request.Content, cancellationToken);
    }
}
