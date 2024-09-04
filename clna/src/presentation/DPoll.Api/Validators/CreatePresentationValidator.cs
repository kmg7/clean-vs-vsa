using Dpoll.Api.Requests;
using FluentValidation;

namespace Dpoll.Api.Validators;
public class CreatePresentationValidator : AbstractValidator<UpdatePresentationRequest>
{
    public CreatePresentationValidator()
    {
        _ = RuleFor(r => r.UserId).NotEqual(Guid.Empty).WithMessage("An user id was not supplied to Update the review.");
        _ = RuleFor(r => r.Title).NotEqual(string.Empty).WithMessage("A title cannot be empty");
    }
}
