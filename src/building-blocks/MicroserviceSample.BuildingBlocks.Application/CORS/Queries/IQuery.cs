using MediatR;

namespace MicroserviceSample.BuildingBlocks.Application.CORS.Queries;

public interface IQuery<out T> : IRequest<T>
    where T : notnull
{
}
