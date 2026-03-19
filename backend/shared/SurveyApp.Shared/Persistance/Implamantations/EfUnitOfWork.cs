using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SurveyApp.Shared.Persistance.Interfaces;

namespace SurveyApp.Shared.Persistance.Implamantations
{
    public class EfUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private readonly TContext _context;
        private IDbContextTransaction _transaction;

        public EfUnitOfWork(TContext context) => _context = context;

        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
                _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync().AsTask();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync().AsTask();
                _transaction = null;
            }
        }

        public void Rollback()
        {
            var entries = _context.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Unchanged)
                .ToList();

            foreach (var entry in entries)
                entry.State = EntityState.Unchanged;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
