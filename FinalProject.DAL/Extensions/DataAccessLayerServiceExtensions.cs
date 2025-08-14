using FinalProject.DAL.DAL;
using FinalProject.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinalProject.DAL.Extensions;

public static class DataAccessLayerServiceExtensions
{
    public static IServiceCollection AddDataAccessLayerServices(this IServiceCollection services)
    {
        //add entity framework core and sql server
        services.AddDbContext<FinalProjectContext>(options =>
            options.UseSqlServer("Server=localhost;Database=FinalProjectDB;User Id=sa;Password=Password123;TrustServerCertificate=True;"));

        // services.AddDbContext<FinalProjectContext>(options =>
        //     options.UseSqlServer(services.BuildServiceProvider()
        //         .GetRequiredService<IConfiguration>()
        //         .GetConnectionString("FinalProjectConnectionString")));
        
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<FinalProjectContext>()
        .AddDefaultTokenProviders();

        // Register Data Access Layer services
        services.AddScoped<ICar, CarDAL>();
        services.AddScoped<ICustomer, CustomerDAL>();
        services.AddScoped<IDealer, DealerDAL>();
        services.AddScoped<IDealerInventory, DealerInventoryDAL>();
        services.AddScoped<ILetterOfIntent, LetterOfIntentDAL>();
        services.AddScoped<ISalesAgreement, SalesAgreementDAL>();
        services.AddScoped<ISalesPerson, SalesPersonDAL>();
        services.AddScoped<IUsman, UsmanDAL>();
        
        return services;
    }
}
