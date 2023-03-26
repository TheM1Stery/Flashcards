using FastEndpoints;
using FluentValidation;

namespace Flashcards.API.Endpoints.Tags.Post;

public class Request
{
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
    public static CreateTagCommand ToCommand(this Request req) => new()
    {
        Name = req.Name
    };
}