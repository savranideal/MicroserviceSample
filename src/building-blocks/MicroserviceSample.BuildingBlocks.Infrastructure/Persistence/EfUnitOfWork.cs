using System.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace MicroserviceSample.BuildingBlocks.Infrastructure.Persistence
{
    public class EfUnitOfWork<TDbContext> : IEfUnitOfWork<TDbContext>
                where TDbContext : DbContext
    {
        private readonly TDbContext _context;
        private readonly ILogger<EfUnitOfWork<TDbContext>> _logger;
        private IDbContextTransaction? _currentTransaction;
        public EfUnitOfWork(TDbContext context, ILogger<EfUnitOfWork<TDbContext>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public TDbContext DbContext => _context;

        public DbSet<TEntity> Set<TEntity>()
            where TEntity : class
        {
            return _context.Set<TEntity>();
        }

        public async Task BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
            _currentTransaction ??= await DbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _currentTransaction ??= await DbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await DbContext.SaveChangesAsync(cancellationToken);
                await _currentTransaction?.CommitAsync(cancellationToken)!;
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _currentTransaction?.RollbackAsync(cancellationToken)!;
            }
            finally
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}