using Flashcards.API.Domain.Cards;

namespace Flashcards.API.Endpoints.Cards.Contracts;

public class CardResponse
{
    public required Guid Id { get; init; }

    public required string Question { get; init; }

    public required string Answer { get; init; }
}

public static class Mapper 
{
    public static CardResponse ToResponse(this Card card)
    {
        return new CardResponse
        {
            Id = (Guid)card.Id,
            Question = card.Question.Value,
            Answer = card.Answer.Value
        };
    }
} 