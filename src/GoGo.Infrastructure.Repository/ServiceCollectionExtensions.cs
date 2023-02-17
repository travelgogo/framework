using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GoGo.Infrastructure.Repository
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepository<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWorkBase<TDbContext>));

            return services;
        }
    }
}
