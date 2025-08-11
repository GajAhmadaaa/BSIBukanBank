using FinalProject.BL.BL;
using FinalProject.BL.Helpers;
using FinalProject.BL.Interfaces;
using FinalProject.BL.Profiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace FinalProject.BL.Extensions
{
    public static class BusinessLogicLayerServiceExtensions
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // Menambahkan AutoMapper dengan license key
            services.AddAutoMapper(cfg => cfg.LicenseKey = "YOUR_LICENSE_KEY_HERE",
            typeof(BusinessLogicLayerServiceExtensions));
            
            //add jwt token
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            if (string.IsNullOrEmpty(appSettings?.Secret))
            {
                throw new ArgumentNullException(nameof(appSettings.Secret), "AppSettings Secret cannot be null or empty");
            }
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            if (key.Length == 0)
            {
                throw new ArgumentException("AppSettings Secret must be a valid non-empty string", nameof(appSettings.Secret));
            }
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            // Mendaftarkan layanan BL
            services.AddScoped<ISalesAgreementBL, SalesAgreementBL>();
            services.AddScoped<ILetterOfIntentBL, LetterOfIntentBL>();
            services.AddScoped<IDealerInventoryBL, DealerInventoryBL>();
            services.AddScoped<ISalesPersonBL, SalesPersonBL>();
            services.AddScoped<ICarBL, CarBL>();
            services.AddScoped<ICustomerBL, CustomerBL>();
            services.AddScoped<IDealerBL, DealerBL>();
            services.AddScoped<IUsmanBL, UsmanBL>();
            
            return services;
        }
    }
}