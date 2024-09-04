using Dpoll.Api.Requests;
using FluentValidation;

namespace Dpoll.Api.Validators;
public class CreateSlideValidator : AbstractValidator<CreateSlideRequest>
{
    public CreateSlideValidator()
    {

        _ = RuleFor(r => r.Type)
            .NotEqual(string.Empty)
            .WithMessage("A Slide type was not supplied to create the Slide.");

        // TODO slide content validation here
    }
}
