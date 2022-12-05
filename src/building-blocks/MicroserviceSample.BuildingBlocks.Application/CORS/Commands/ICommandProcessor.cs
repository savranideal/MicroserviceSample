namespace MicroserviceSample.BuildingBlocks.Application.CORS.Commands;

public interface ICommandProcessor
{
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        where TResult : notnull;
}
