
using Dpoll.Api.Requests;
using FluentValidation;

namespace Dpoll.Api.Validators;
public class UpdateSlideValidator : AbstractValidator<UpdateSlideIndexRequest>
{
    public UpdateSlideValidator()
    {
        _ = RuleFor(r => r.Index)
            .GreaterThanOrEqualTo(0)
            .WithMessage("A Slide index cannot be negative.");
    }
}
