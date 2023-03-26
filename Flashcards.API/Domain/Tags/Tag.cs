using Flashcards.API.Domain.Cards;
using Flashcards.API.Domain.Common;

namespace Flashcards.API.Domain.Tags;

public class Tag
{
    public Id Id { get; init; }

    public Name Name { get; init; }
    
    public List<Card>? Cards { get; set; }
}