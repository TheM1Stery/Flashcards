using Flashcards.API.Domain.Common;
using Flashcards.API.Domain.Tags;

namespace Flashcards.API.Domain.Cards;

public class Card
{
    public Id Id { get; init; }

    public Question Question { get; init; }
    
    public Answer Answer { get; init; }

    public List<Tag> Tags { get; set; } = new();
}