namespace MicroserviceSample.BuildingBlocks.Infrastructure.Persistence
{
    public interface ITransactionable
    {
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    }
}