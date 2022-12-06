using MediatR;
using MicroserviceSample.BuildingBlocks.Application.CORS.Commands;

namespace MicroserviceSample.Services.Contacts.Infrastructure.Configuration.Command
{
    internal class CommandProcessor : ICommandProcessor
    {
        private readonly IMediator _mediator;

        public CommandProcessor(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<TResult> SendAsync<TResult>(
            ICommand<TResult> command,
            CancellationToken cancellationToken = default)
            where TResult : notnull
        {
            return _mediator.Send(command, cancellationToken);
        }
    }
}
