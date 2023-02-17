namespace GoGo.Infrastructure.Repository
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> Repo<TEntity>() where TEntity : class, IEntity;
        Task BeginTransactionAsync();
        Task RollbackTransactionAsync();
        Task CommitTransactionAsync();
        Task<bool> SaveChangeAsync();
    }
}