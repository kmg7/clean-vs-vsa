namespace Presentation.Validators;

using FluentValidation;
using Presentation.Requests;

public class UpdateSlideValidator : AbstractValidator<UpdateSlideIndexRequest>
{
    public UpdateSlideValidator()
    {
        _ = this.RuleFor(r => r.Index)
            .GreaterThanOrEqualTo(0)
            .WithMessage("A Slide index cannot be negative.");
    }
}
