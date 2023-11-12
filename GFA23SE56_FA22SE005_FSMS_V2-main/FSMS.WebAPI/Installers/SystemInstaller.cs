using FSMS.Service.Services.CacheServices;

namespace FSMS.WebAPI.Installers
{
    public class SystemInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }
    }
}
