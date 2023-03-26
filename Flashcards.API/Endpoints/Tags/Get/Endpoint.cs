using System.Data;
using Dapper;
using FastEndpoints;
using Flashcards.API.Domain.Tags;
using Flashcards.API.Endpoints.Tags.Contracts;
using Mediator;
using OneOf;
using OneOf.Types;

namespace Flashcards.API.Endpoints.Tags.Get;

public class GetTagByIdQuery : IQuery<OneOf<Tag, NotFound>>
{
    public required Guid Id { get; init; }
}

public class GetTagByIdQueryHandler : IQueryHandler<GetTagByIdQuery, OneOf<Tag, NotFound>>
{
    private readonly IDbConnection _dbConnection;

    public GetTagByIdQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async ValueTask<OneOf<Tag, NotFound>> Handle(GetTagByIdQuery query, CancellationToken cancellationToken)
    {
        var tag = await _dbConnection.QuerySingleOrDefaultAsync<Tag>("SELECT * FROM Tags WHERE Id = @Id", query);
        if (tag is null)
            return new NotFound();
        return tag;
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
        Get("/{id}");
        Group<TagsGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var result = await _mediator.Send(req.ToQuery(), ct);
        await result.Match(
            x => SendAsync(x.ToResponse(), cancellation: ct),
            nf => SendNotFoundAsync(ct)
        );
    }
}