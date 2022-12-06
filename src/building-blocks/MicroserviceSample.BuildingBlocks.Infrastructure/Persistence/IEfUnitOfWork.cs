using Microsoft.EntityFrameworkCore;

namespace MicroserviceSample.BuildingBlocks.Infrastructure.Persistence
{
    public interface IEfUnitOfWork : IUnitOfWork, ITransactionable
    {
    }

    public interface IEfUnitOfWork<out TContext> : IEfUnitOfWork
        where TContext : DbContext
    {
        TContext DbContext { get; }
    }
}