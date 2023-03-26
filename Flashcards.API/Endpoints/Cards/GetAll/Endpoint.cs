using System.Data;
using Dapper;
using FastEndpoints;
using Flashcards.API.Domain.Cards;
using Flashcards.API.Domain.Tags;
using Flashcards.API.Endpoints.Cards.Contracts;
using Mediator;

namespace Flashcards.API.Endpoints.Cards.GetAll;

public class GetAllCardsQuery : IQuery<Card[]>
{
}

public class GetAllCardsQueryHandler : IQueryHandler<GetAllCardsQuery, Card[]>
{
    private readonly IDbConnection _dbConnection;

    public GetAllCardsQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async ValueTask<Card[]> Handle(GetAllCardsQuery query, CancellationToken cancellationToken)
    {
        var cards = await _dbConnection.QueryAsync<Card, Tag, Card>(
            "SELECT C.Id as Id, C.Question, C.Answer,T.Id as Id, T.Name " +
            "FROM Cards C " +
            "INNER JOIN CardTags CT on C.Id = CT.CardId " +
            "INNER JOIN Tags T on T.Id = CT.TagId", 
            (card, tag) =>
            {
                card.Tags.Add(tag);
                return card;
            });
        var groupedCards = cards.GroupBy(x => x.Id).Select(x =>
        {
            var card = x.First();
            card.Tags = x.Select(y => y.Tags.First()).ToList();
            return card;
        });

        return groupedCards.ToArray();
    }
}

public class Endpoint : EndpointWithoutRequest<Response>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("");
        Group<CardsGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var cards = await _mediator.Send(new GetAllCardsQuery(), ct);
        await SendAsync(new Response
        {
            Cards = cards.Select(x => x.ToResponse()).ToArray()
        }, cancellation: ct);
    }
}