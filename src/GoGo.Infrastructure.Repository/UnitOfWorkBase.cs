using Microsoft.EntityFrameworkCore;

namespace GoGo.Infrastructure.Repository
{
    public class UnitOfWorkBase<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        private readonly TDbContext _context;
        private readonly Dictionary<Type, object> _repos;

        public UnitOfWorkBase(TDbContext context)
        {
            _context = context;
            _repos = new Dictionary<Type, object>();
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public IRepository<TEntity> Repo<TEntity>() where TEntity : class, IEntity
        {
            var type = typeof(TEntity);
            if(!_repos.ContainsKey(type))
            {
                _repos.Add(type, new RepositoryBase<TEntity>(_context));
            }

            return (IRepository<TEntity>)_repos[type];
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}