using FastEndpoints;
using Flashcards.API.Domain.Cards;
using FluentValidation;

namespace Flashcards.API.Endpoints.Cards.Get;

public class Request
{
    public required Guid Id { get; init; }
}

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class Response
{
    public Guid Id { get; init; }
    public required string Question { get; init; }
    public required string Answer { get; init; }
    public required string[] Tags { get; init; }
}

public static class Mapper
{
    public static Response Map(this Card card)
    {
        return new()
        {
            Id = (Guid)card.Id,
            Question = (string)card.Question,
            Answer = (string)card.Answer,
            Tags = card.Tags.Select(x => x.Name.Value).ToArray()
        };
    }
}