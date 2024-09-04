using FluentValidation;

namespace Dpoll.Api.Validators;
public class GenericIdentityValidator : AbstractValidator<Guid>
{
    public GenericIdentityValidator()
    {
        _ = RuleFor(r => r)
            .NotNull()
            .NotEqual(Guid.Empty)
            .WithMessage("The Id supplied in the request is not valid.");
    }
}
