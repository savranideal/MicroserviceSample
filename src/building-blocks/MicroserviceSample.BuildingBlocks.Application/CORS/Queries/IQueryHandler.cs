using MediatR;

namespace MicroserviceSample.BuildingBlocks.Application.CORS.Queries;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : notnull
{
} 
