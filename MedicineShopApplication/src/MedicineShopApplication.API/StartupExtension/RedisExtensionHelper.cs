namespace MedicineShopApplication.API.StartupExtension
{
    public static class RedisExtensionHelper
    {
        public static IServiceCollection AddRedisExtensionHelper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetValue<string>("Redis:SecondaryConnection");
                options.InstanceName = "SampleInstance";
            });

            return services;
        }
    }
}
