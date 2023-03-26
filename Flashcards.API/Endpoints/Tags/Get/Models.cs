using FastEndpoints;
using FluentValidation;

namespace Flashcards.API.Endpoints.Tags.Get;

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
    public static GetTagByIdQuery ToQuery(this Request req)
    {
        return new GetTagByIdQuery
        {
            Id = req.Id
        };
    }
}