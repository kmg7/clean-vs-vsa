using Dpoll.Domain.Common.Enums;

namespace DPoll.Domain.Common.Errors
{
    public sealed class Error
    {
        public string Code { get; }
        public string Description { get; }
        public Error(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public static readonly Error None = new Error(string.Empty, string.Empty);

        public static Error NotFound(EntityType entityType)
        {
            return new Error(ErrorCodes.NotFound, $"{entityType} {ErrorMessages.NotFound}");
        }
    }
}
