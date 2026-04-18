using GoodHamburger.Application.Interfaces;
using GoodHamburger.Application.Mappings;
using GoodHamburger.Application.Services;
using GoodHamburger.Domain.Factory;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Strategies;
using GoodHamburger.Infra.Data.Context;
using GoodHamburger.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburger.Infra.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            services.AddRepositories();
            services.AddDomainServices();
            services.AddApplicationServices();
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

            return services;
        }

       private static void AddDatabase(this IServiceCollection services,IConfiguration configuration)
       {
           services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
       }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();
        }

        private static void AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IDiscountStrategy, FullComboStrategy>();
            services.AddScoped<IDiscountStrategy, SandwichWithDrinkStrategy>();
            services.AddScoped<IDiscountStrategy, SandwichWithFriesStrategy>();
            services.AddScoped<IDiscountStrategy, NoDiscountStrategy>();

            services.AddScoped<DiscountStrategyResolver>();
            services.AddScoped<OrderFactory>();
        }

        private static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IMenuItemService, MenuItemService>();
        }
    }
}
