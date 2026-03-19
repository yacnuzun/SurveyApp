namespace SurveyApp.Shared.Persistance.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        void Rollback();
    }
}
