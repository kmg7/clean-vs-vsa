using FluentValidation;

namespace Dpoll.Api.Filters;
public class ValidationDescriptor
{
    public required int ArgumentIndex { get; init; }
    public required Type ArgumentType { get; init; }
    public required IValidator Validator { get; init; }
}
