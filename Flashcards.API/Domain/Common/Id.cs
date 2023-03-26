using Vogen;

namespace Flashcards.API.Domain.Common;

[ValueObject<Guid>(Conversions.DapperTypeHandler)]
[Instance("Unspecified", "Guid.Empty")]
public readonly partial struct Id
{
    private static Validation Validate(Guid input)
    {
        return input != Guid.Empty ? Validation.Ok : Validation.Invalid("Id cannot be empty");
    }
}