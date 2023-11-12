using FSMS.Service.Services.CacheServices;

namespace FSMS.WebAPI.Installers
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IResponseCacheService, ResponseCacheService>();
        }
    }
}
