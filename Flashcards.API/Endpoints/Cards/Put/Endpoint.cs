using System.Data;
using Dapper;
using FastEndpoints;
using Flashcards.API.Domain.Cards;
using Flashcards.API.Domain.Common;
using Flashcards.API.Endpoints.Cards.Contracts;
using Mediator;
using OneOf;
using OneOf.Types;

namespace Flashcards.API.Endpoints.Cards.Put;

public class UpdateCardCommand : Mediator.ICommand<OneOf<Card, NotFound>>
{
    public required Guid Id { get; init; }

    public required string Question { get; init; }

    public required string Answer { get; init; }
}

public class UpdateCardCommandHandler : Mediator.ICommandHandler<UpdateCardCommand, OneOf<Card, NotFound>>
{
    private readonly IDbConnection _dbConnection;

    public UpdateCardCommandHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async ValueTask<OneOf<Card, NotFound>> Handle(UpdateCardCommand command, CancellationToken cancellationToken)
    {
        var rows = await _dbConnection.ExecuteAsync(
            "UPDATE Cards SET Question = @Question, Answer = @Answer WHERE Id = @Id",
            new { command.Id, command.Question, command.Answer });
        if (rows == 0)
            return new NotFound();
        return new Card
        {
            Answer = (Answer)command.Answer,
            Question = (Question)command.Question,
            Id = (Id)command.Id
        };
    }
}

public class Endpoint : Endpoint<Request, CardResponse>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/{id}");
        Group<CardsGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var command = await _mediator.Send(new UpdateCardCommand
        {
            Id = req.Id,
            Question = req.Question,
            Answer = req.Answer
        }, ct);
        await command.Match(
            x => SendOkAsync(x.ToResponse(), ct),
            nf => SendNotFoundAsync(ct)
        );
    }
}