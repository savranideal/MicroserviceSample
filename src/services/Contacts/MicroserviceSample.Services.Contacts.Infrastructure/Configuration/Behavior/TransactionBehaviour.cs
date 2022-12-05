using MediatR;
using MicroserviceSample.BuildingBlocks.Application.CORS.Commands;
using MicroserviceSample.BuildingBlocks.Infrastructure.Persistence;
using MicroserviceSample.Services.Contacts.Infrastructure.Configuration.Extensions;
using Microsoft.Extensions.Logging;

namespace MicroserviceSample.Services.Contacts.Infrastructure.Configuration.Behavior
{
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : ICommand<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;

        public TransactionBehaviour(IUnitOfWork unitOfWork, ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger ?? throw new ArgumentException(nameof(ILogger));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse? response = default;
            var typeName = request.GetGenericTypeName();
             
            await next();

            await _unitOfWork.CommitAsync(cancellationToken);

            return response;
        }
    }
}
