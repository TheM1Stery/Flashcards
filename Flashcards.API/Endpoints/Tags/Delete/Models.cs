using FastEndpoints;
using FluentValidation;

namespace Flashcards.API.Endpoints.Tags.Delete;

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


public static class Mapper
{
    public static DeleteTagCommand ToCommand(this Request req)
    {
        return new()
        {
            Id = req.Id
        };
    }
}