using Flashcards.API.Domain.Tags;
using Flashcards.API.Endpoints.Tags.Contracts;

namespace Flashcards.API.Endpoints.Tags.GetAll;

public class Response
{
    public required TagResponse[] Tags { get; init; }
}