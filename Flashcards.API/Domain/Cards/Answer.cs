using Vogen;

namespace Flashcards.API.Domain.Cards;

[ValueObject<string>(Conversions.DapperTypeHandler)]
public readonly partial struct Answer
{
    private static Validation Validate(string input)
    {
        return !string.IsNullOrWhiteSpace(input) ? Validation.Ok : Validation.Invalid("Answer must not be empty");
    }
}