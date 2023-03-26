using FastEndpoints;
using FluentValidation;

namespace Flashcards.API.Endpoints.Tags.Put;

public class Request
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public static class Mapper
{
    public static UpdateTagCommand ToCommand(this Request req)
    {
        return new()
        {
            Id = req.Id,
            Name = req.Name
        };
    }
}