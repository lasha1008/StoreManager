using Microsoft.EntityFrameworkCore;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Facade.Interfaces.Services;
using StoreManager.Repositories;
using StoreManager.Services;
using StoreManager.Web.Helpers;
using StoreManager.Web.Interfaces;

namespace StoreManager.Web.Configuration;

public static class DependencyConfigurator
{
    public static void ConfigureDependency(this WebApplicationBuilder builder, ConfigurationManager configuration)
    {
        builder.Services.AddScoped<IApiHelper, ApiHelper>();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddTransient<ICategoryQueryService, CategoryQueryService>();
        builder.Services.AddTransient<ICategoryCommandService, CategoryCommandService>();

        builder.Services.AddTransient<IProductQueryService, ProductQueryService>();
        builder.Services.AddTransient<IProductCommandService, ProductCommandService>();

        builder.Services.AddTransient<ICountryQueryService, CountryQueryService>();
        builder.Services.AddTransient<ICountryCommandService, CountryCommandService>();

        builder.Services.AddTransient<ICityQueryService, CityQueryService>();
        builder.Services.AddTransient<ICityCommandService, CityCommandService>();

        builder.Services.AddTransient<IEmployeeQueryService, EmployeeQueryService>();
        builder.Services.AddTransient<IEmployeeCommandService, EmployeeCommandService>();

        builder.Services.AddTransient<ICustomerQueryService, CustomerQueryService>();
        builder.Services.AddTransient<ICustomerCommandService, CustomerCommandService>();

        builder.Services.AddTransient<IOrderQueryService, OrderQueryService>();
        builder.Services.AddTransient<IOrderCommandService, OrderCommandService>();

        builder.Services.AddTransient<ICustomerAccountService, CustomerAccountService>();

        builder.Services.AddDbContext<StoreManagerDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("StoreManager")));
    }
}
