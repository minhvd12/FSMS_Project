namespace FSMS.WebAPI.Installers
{
    public class DistributedCacheInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // Cấu hình IDistributedCache ở đây
            services.AddDistributedMemoryCache(); // Hoặc thay bằng cấu hình Redis nếu bạn muốn sử dụng Redis làm Distributed Cache
        }
    }
}
