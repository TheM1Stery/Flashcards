using Vogen;

namespace Flashcards.API.Domain.Tags;

[ValueObject<string>(Conversions.DapperTypeHandler)]
public readonly partial struct Name
{
    private static Validation Validate(string input)
    {
        return !string.IsNullOrWhiteSpace(input)
            ? Validation.Ok
            : Validation.Invalid("Name cannot be empty");
    }
}