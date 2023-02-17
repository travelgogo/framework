using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoGo.Infrastructure.BlobStorage
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Get key BlobStorage:Azure from appsetting to initialize a BlobStorageManager
        /// </summary>
        public static IServiceCollection AddBlobStorage(this IServiceCollection services)
        {
            services.AddSingleton(service =>
            {
                var config = service.GetRequiredService<IConfiguration>();
                var option = new BlobStorageOptions();
                var blobSection = config.GetSection("BlobStorage:Azure");
                blobSection.Bind(option);
                return option;
            });
            services.AddSingleton<IBlobStorageManager, BlobStorageManager>();
            return services;
        }
    }
}