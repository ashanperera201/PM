using Microsoft.Extensions.DependencyInjection;
using PM.CORE.Interfaces;
using PM.SERVICES.Delegates;
using PM.SERVICES.Mapping;
using PM.SERVICES.Services;


namespace PM.SERVICES
{
    public static class ServiceDependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(typeof(ProductMappingProfile));


            // Register delegate handlers
            services.AddScoped(serviceProvider =>
            {
                var productService = serviceProvider.GetRequiredService<IProductService>();
                var delegateHandler = serviceProvider.GetRequiredService<ProductDelegateHandler>();
                delegateHandler.RegisterHandlers(productService);
                return productService;
            });


            // Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ProductDelegateHandler>();


            return services;
        }
    }
}
