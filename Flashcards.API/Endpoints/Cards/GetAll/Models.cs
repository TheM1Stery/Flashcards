using Flashcards.API.Endpoints.Cards.Contracts;

namespace Flashcards.API.Endpoints.Cards.GetAll;

public class Response 
{
    public required CardResponse[] Cards { get; init; }
}