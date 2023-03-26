using System.Data;
using Dapper;
using FastEndpoints;
using Flashcards.API.Domain.Common;
using Flashcards.API.Domain.Tags;
using Flashcards.API.Endpoints.Tags.Contracts;
using Mediator;
using OneOf;
using OneOf.Types;

namespace Flashcards.API.Endpoints.Tags.Put;

public class UpdateTagCommand : Mediator.ICommand<OneOf<Tag, NotFound>>
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}

public class UpdateTagCommandHandler : Mediator.ICommandHandler<UpdateTagCommand, OneOf<Tag, NotFound>>
{
    private readonly IDbConnection _connection;

    public UpdateTagCommandHandler(IDbConnection connection)
    {
        _connection = connection;
    }

    public async ValueTask<OneOf<Tag, NotFound>> Handle(UpdateTagCommand command, CancellationToken cancellationToken)
    {
        var rows = await _connection.ExecuteAsync("UPDATE Tags SET Name = @Name WHERE Id = @Id", command);
        return rows > 0 ? new Tag
        {
            Id = (Id)command.Id,
            Name = (Name)command.Name
        } : new NotFound();
    }
}

public class Endpoint : Endpoint<Request>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("");
        Group<TagsGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var result = await _mediator.Send(req.ToCommand(), ct);
        await result.Match(
            t => SendAsync(t.ToResponse(), cancellation: ct),
            nf => SendNotFoundAsync(ct)
        );
    }
}