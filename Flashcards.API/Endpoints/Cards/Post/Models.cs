using FastEndpoints;
using FluentValidation;

namespace Flashcards.API.Endpoints.Cards.Post;

public class Request
{
    public required string Question { get; init; }
    public required string Answer { get; init; }
}

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Answer).NotEmpty();
        RuleFor(x => x.Question).NotEmpty();
    }
}