namespace Api.Features.Presentations.Commands;

using Api.Common;
using Api.Common.Enums;
using Api.Common.Exceptions;
using Api.Features.Presentations.Persistence;
using Api.Features.Users.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

public partial class PresentationsController : ApiControllerBase
{
    [HttpPut("/api/presentations/{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePresentationRequest request)
    {
        try
        {
            _ = await Mediator.Send(new UpdateCommand
            {
                Id = id,
                UserId = request.UserId,
                Title = request.Title
            });

            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}

public class UpdateCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; init; }
}

public class UpdatePresentationRequest
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
}

public class UpdatePresentationValidator : AbstractValidator<UpdatePresentationRequest>
{
    public UpdatePresentationValidator()
    {
        _ = RuleFor(r => r.UserId).NotEqual(Guid.Empty).WithMessage("An user id was not supplied to Update the review.");
        _ = RuleFor(r => r.Title).NotEqual(string.Empty).WithMessage("A title cannot be empty");
    }
}

public class UpdateCommandHandler(
    IUsersRepository UsersRepository,
    IPresentationsRepository PresentationsRepository) : IRequestHandler<UpdateCommand, bool>
{
    public async Task<bool> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        if (!await PresentationsRepository.PresentationExists(request.Id, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Presentation);
        }

        if (!await UsersRepository.UserExists(request.UserId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.User);
        }

        return await PresentationsRepository
            .UpdatePresentation(request.Id, request.UserId, request.Title, cancellationToken);
    }
}
