using System.Data;
using Dapper;
using FastEndpoints;
using Mediator;
using OneOf;
using OneOf.Types;

namespace Flashcards.API.Endpoints.Cards.Delete;

public class DeleteCardCommand : Mediator.ICommand<OneOf<Success, NotFound>>
{
    public required Guid Id { get; init; }
}

public class DeleteCardCommandHandler : Mediator.ICommandHandler<DeleteCardCommand, OneOf<Success, NotFound>>
{
    private readonly IDbConnection _dbConnection;

    public DeleteCardCommandHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async ValueTask<OneOf<Success, NotFound>> Handle(DeleteCardCommand command,
        CancellationToken cancellationToken)
    {
        var rows = await _dbConnection.ExecuteAsync("DELETE FROM Cards WHERE Id = @Id", new { command.Id });
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
        Group<CardsGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteCardCommand()
        {
            Id = req.Id
        }, ct);
        await result.Match(
            s => SendNoContentAsync(ct),
            nf => SendNotFoundAsync(ct)
        );
    }
}