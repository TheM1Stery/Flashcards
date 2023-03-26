using System.Data;
using Dapper;
using FastEndpoints;
using Flashcards.API.Domain.Cards;
using Flashcards.API.Domain.Tags;
using Flashcards.API.Endpoints.Cards.Contracts;
using Mediator;
using OneOf;
using OneOf.Types;

namespace Flashcards.API.Endpoints.Cards.Get;

public class GetCardByIdQuery : IQuery<OneOf<Card, NotFound>>
{
    public required Guid Id { get; init; }
}

public class GetCardQueryHandler : IQueryHandler<GetCardByIdQuery, OneOf<Card, NotFound>>
{
    private readonly IDbConnection _dbConnection;

    public GetCardQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async ValueTask<OneOf<Card, NotFound>> Handle(GetCardByIdQuery query, CancellationToken cancellationToken)
    {
        var cards = (await _dbConnection.QueryAsync<Card, Tag, Card>(
            "SELECT C.Id as Id, C.Question, C.Answer,T.Id as Id, T.Name " +
            "FROM Cards as C " +
            "INNER JOIN CardTags CT on C.Id = CT.CardId " +
            "INNER JOIN Tags T on T.Id = CT.TagId " +
            "WHERE C.Id == @Id", (card, tag) =>
            {
                card.Tags.Add(tag);
                return card;
            }, query)).ToArray();
        if (cards.Length == 0)
            return new NotFound();
        var card = cards[0];
        card.Tags = cards.Select(x => x.Tags.First()).ToList();
        return card;
    }
}

public class Endpoint : Endpoint<Request, Response>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/{id}");
        Group<CardsGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCardByIdQuery()
        {
            Id = req.Id
        }, ct);
        await result.Match(
            c => SendAsync(c.Map(), cancellation: ct),
            nf => SendNotFoundAsync(ct)
        );
    }
}