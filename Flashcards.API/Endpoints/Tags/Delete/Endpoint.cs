using System.Data;
using Dapper;
using FastEndpoints;
using Mediator;
using OneOf;
using OneOf.Types;

namespace Flashcards.API.Endpoints.Tags.Delete;

public class DeleteTagCommand : Mediator.ICommand<OneOf<Success, NotFound>>
{
    public required Guid Id { get; init; }
}

public class DeleteTagCommandHandler : Mediator.ICommandHandler<DeleteTagCommand, OneOf<Success, NotFound>>
{
    private readonly IDbConnection _connection;

    public DeleteTagCommandHandler(IDbConnection connection)
    {
        _connection = connection;
    }

    public async ValueTask<OneOf<Success, NotFound>> Handle(DeleteTagCommand command,
        CancellationToken cancellationToken)
    {
        var rows = await _connection.ExecuteAsync("DELETE FROM Tags WHERE Id = @Id", command);
        return rows > 0 ? new Success() : new NotFound();
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
        Delete("/{id}");
        Group<TagsGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var result = await _mediator.Send(req.ToCommand(), ct);
        await result.Match(
            s => SendNoContentAsync(ct),
            nf => SendNotFoundAsync(ct)
        );
    }
}