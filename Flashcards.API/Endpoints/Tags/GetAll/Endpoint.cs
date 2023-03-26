using System.Data;
using Dapper;
using FastEndpoints;
using Flashcards.API.Domain.Tags;
using Flashcards.API.Endpoints.Tags.Contracts;
using Mediator;

namespace Flashcards.API.Endpoints.Tags.GetAll;

public class GetAllTagsQuery : IQuery<Tag[]>
{
}

public class GetAllTagsQueryHandler : IQueryHandler<GetAllTagsQuery, Tag[]>
{
    private readonly IDbConnection _dbConnection;

    public GetAllTagsQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async ValueTask<Tag[]> Handle(GetAllTagsQuery query, CancellationToken cancellationToken)
    {
        var tags = await _dbConnection.QueryAsync<Tag>("SELECT * FROM Tags");
        return tags.ToArray();
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
        Group<TagsGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllTagsQuery(), ct);
        await SendAsync(new Response
        {
            Tags = result.Select(x => x.ToResponse()).ToArray()
        }, cancellation: ct);
    }
}