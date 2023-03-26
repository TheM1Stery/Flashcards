using Vogen;

namespace Flashcards.API.Domain.Cards;

[ValueObject<string>(Conversions.DapperTypeHandler)]
public readonly partial struct Question 
{
    private static Validation Validate(string input)
    {
        return !string.IsNullOrWhiteSpace(input) ? Validation.Ok : Validation.Invalid("Question must not be empty");
    }
}