using MediatR;
using MicroserviceSample.BuildingBlocks.Application.CORS.Queries;

namespace MicroserviceSample.Services.Contacts.Infrastructure.Configuration.Query;

internal class QueryProcessor : IQueryProcessor
{
    private readonly IMediator _mediator;

    public QueryProcessor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default) where TResponse : notnull
    {
        return _mediator.Send(query, cancellationToken);
    }
}
