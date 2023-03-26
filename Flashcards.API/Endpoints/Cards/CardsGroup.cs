using FastEndpoints;

namespace Flashcards.API.Endpoints.Cards;

public sealed class CardsGroup : Group
{
    public CardsGroup()
    {
        Configure("cards", ep =>
        {
            ep.AllowAnonymous();
        });
    }
}