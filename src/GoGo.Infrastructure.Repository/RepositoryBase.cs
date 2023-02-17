using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace GoGo.Infrastructure.Repository
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly DbContext _context;
        protected DbSet<TEntity> DbSet => _context.Set<TEntity>();

        public RepositoryBase(DbContext context)
        {
            _context = context;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        public async Task<TEntity?> GetAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TEntity?> GetAsync(int id, string[]? includeProperties = null)
        {
            IQueryable<TEntity> query = DbSet.AsQueryable();
            if (includeProperties != null && includeProperties.Any())
            {
                foreach (var p in includeProperties)
                {
                    query = query.Include(p);
                }
            }
            return await query.FirstOrDefaultAsync(x=> x.Id == id);
        }

        public IQueryable<TEntity> GetAsync(Expression<Func<TEntity, bool>>? predicate = null, string[]? includeProperties = null)
        {
            IQueryable<TEntity> query = DbSet.AsQueryable();
            if (includeProperties != null && includeProperties.Any())
            {
                foreach (var p in includeProperties)
                {
                    query = query.Include(p);
                }
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return query;
        }

        public async Task RemoveAsync(TEntity entity)
        {
            await Task.Yield();
            DbSet.Remove(entity);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var trackingEntity = await DbSet.FirstOrDefaultAsync(x => x.Id == entity.Id);
            DbSet.Update(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}