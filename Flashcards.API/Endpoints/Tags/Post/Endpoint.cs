using System.Data;
using Dapper;
using FastEndpoints;
using Flashcards.API.Domain.Common;
using Flashcards.API.Domain.Tags;
using Flashcards.API.Endpoints.Tags.Contracts;
using Mediator;

namespace Flashcards.API.Endpoints.Tags.Post;

public class CreateTagCommand : Mediator.ICommand<Tag>
{
    public required string Name { get; init; }
}

public class CreateTagCommandHandler : Mediator.ICommandHandler<CreateTagCommand, Tag>
{
    private readonly IDbConnection _dbConnection;

    public CreateTagCommandHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async ValueTask<Tag> Handle(CreateTagCommand command, CancellationToken cancellationToken)
    {
        var guid = Guid.NewGuid();
        await _dbConnection.ExecuteAsync("INSERT INTO Tags (Id, Name) VALUES (@Id,@Name)",
            new { Id = guid, command.Name });
        return new Tag
        {
            Id = (Id)guid,
            Name = (Name)command.Name
        };
    }
}

public class Endpoint : Endpoint<Request, TagResponse>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("");
        Group<TagsGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var tag = await _mediator.Send(req.ToCommand(), ct);
        await SendAsync(tag.ToResponse(), cancellation: ct);
    }
}