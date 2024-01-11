using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Repositories;

namespace StoreManager.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<StoreManagerDbContext>(options => options.UseInMemoryDatabase(databaseName: "MyTestDatabase"));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}