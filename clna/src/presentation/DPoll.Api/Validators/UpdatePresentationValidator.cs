using Dpoll.Api.Requests;
using FluentValidation;

namespace Dpoll.Api.Validators;
public class UpdatePresentationValidator : AbstractValidator<UpdatePresentationRequest>
{
    public UpdatePresentationValidator()
    {
        _ = RuleFor(r => r.UserId).NotEqual(Guid.Empty).WithMessage("An user id was not supplied to Update the review.");
        _ = RuleFor(r => r.Title).NotEqual(string.Empty).WithMessage("A title cannot be empty");
    }
}
