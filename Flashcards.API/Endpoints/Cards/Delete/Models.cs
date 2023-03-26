using FastEndpoints;
using FluentValidation;

namespace Flashcards.API.Endpoints.Cards.Delete;

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