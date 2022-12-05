namespace MicroserviceSample.BuildingBlocks.Application.CORS.Queries;

public interface IQueryProcessor
{
    Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        where TResponse : notnull;
}
