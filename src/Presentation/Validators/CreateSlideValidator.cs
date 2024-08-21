namespace Presentation.Validators;

using FluentValidation;
using Presentation.Requests;

public class CreateSlideValidator : AbstractValidator<CreateSlideRequest>
{
    public CreateSlideValidator()
    {

        _ = this.RuleFor(r => r.Type)
            .NotEqual(string.Empty)
            .WithMessage("A Slide type was not supplied to create the Slide.");

        // TODO slide content validation here
    }
}
