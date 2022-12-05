using MediatR;

namespace MicroserviceSample.BuildingBlocks.Application.CORS.Commands;

public interface ICommand : ICommand<Unit>
{
}

public interface ICommand<out T> : IRequest<T>
    where T : notnull
{
}
