using Serilog;

namespace MedicineShopApplication.API.StartupExtension
{
    public static class SerilogApplicationExtensionHelper
    {
        public static IServiceCollection AddSerilogApplicationExtensionHelper(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            return services;
        }
    }
}
