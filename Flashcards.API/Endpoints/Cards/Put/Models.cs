using FastEndpoints;
using FluentValidation;

namespace Flashcards.API.Endpoints.Cards.Put;

public class Request
{
    public required Guid Id { get; init; }
    
    public required string Question { get; init; }
    
    public required string Answer { get; init; }
}

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Answer).NotEmpty();
        RuleFor(x => x.Question).NotEmpty();
    }
}