namespace Api.Features.Presentations.Commands;

using Api.Common;
using Api.Features.Presentations.Persistence;
using Api.Features.Users.Persistence;
using Common.Enums;
using Common.Exceptions;
using Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

public partial class PresentationsController : ApiControllerBase
{
    [HttpPost("/api/presentations")]
    public async Task<IActionResult> Create([FromBody] CreatePresentationRequest request)
    {
        try
        {
            var response = await Mediator.Send(new CreateCommand
            {
                UserId = request.UserId,
                Title = request.Title
            });

            return Created($"/api/presentation/{response.Id}", response);
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

public class CreateCommand : IRequest<Presentation>
{
    public Guid UserId { get; init; }
    public string Title { get; init; }
}

public class CreatePresentationRequest
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
}

public class CreatePresentationValidator : AbstractValidator<CreatePresentationRequest>
{
    public CreatePresentationValidator()
    {
        _ = RuleFor(r => r.UserId).NotEqual(Guid.Empty).WithMessage("An user id was not supplied to Update the review.");
        _ = RuleFor(r => r.Title).NotEqual(string.Empty).WithMessage("A title cannot be empty");
    }
}

public class CreateCommandHandler(
    IUsersRepository UsersRepository,
    IPresentationsRepository PresentationsRepository) : IRequestHandler<CreateCommand, Presentation>
{
    public async Task<Presentation> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        if (!await UsersRepository.UserExists(request.UserId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.User);
        }

        return await PresentationsRepository
            .CreatePresentation(request.UserId, request.Title, cancellationToken);
    }
}
