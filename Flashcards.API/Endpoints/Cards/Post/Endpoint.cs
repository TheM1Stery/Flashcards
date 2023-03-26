using System.Data;
using Dapper;
using FastEndpoints;
using Mediator;
using Flashcards.API.Domain.Cards;
using Flashcards.API.Domain.Common;
using Flashcards.API.Endpoints.Cards.Contracts;


namespace Flashcards.API.Endpoints.Cards.Post;

public class CreateCardCommand : Mediator.ICommand<Card>
{
    public required string Question { get; init; }

    public required string Answer { get; init; }
}

public class CreateCardCommandHandler : Mediator.ICommandHandler<CreateCardCommand, Card>
{
    private readonly IDbConnection _connection;

    public CreateCardCommandHandler(IDbConnection connection)
    {
        _connection = connection;
    }

    public async ValueTask<Card> Handle(CreateCardCommand command, CancellationToken cancellationToken)
    {
        var guid = Guid.NewGuid();
        await _connection.ExecuteAsync("INSERT INTO Cards (Id, Question, Answer) VALUES (@Id, @Question, @Answer)",
            new { Id = guid, command.Question, command.Answer });
        return new Card
        {
            Id = (Id)guid,
            Answer = (Answer)command.Answer,
            Question = (Question)command.Question
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
        Post("");
        Group<CardsGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateCardCommand
        {
            Question = req.Question,
            Answer = req.Answer
        }, ct);
        await SendOkAsync(result.ToResponse(), ct);
    }
}