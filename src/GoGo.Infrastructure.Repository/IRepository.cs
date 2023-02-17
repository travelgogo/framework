using System.Linq.Expressions;

namespace GoGo.Infrastructure.Repository
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> AddAsync(TEntity e);
        Task<TEntity> UpdateAsync(TEntity e);
        Task RemoveAsync(TEntity e);
        Task<TEntity?> GetAsync(int id);
        Task<TEntity?> GetAsync(int id, string[]? includeProperties = default);
        IQueryable<TEntity> GetAsync(Expression<Func<TEntity, bool>>? predicate = null, string[]? includeProperties = default);
    }
}