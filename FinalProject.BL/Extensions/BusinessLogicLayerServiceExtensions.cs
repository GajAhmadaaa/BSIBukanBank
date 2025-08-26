using FinalProject.BL.BL;
using FinalProject.BL.Helpers;
using FinalProject.BL.Interfaces;
using FinalProject.BL.Profiles;
using FinalProject.DAL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace FinalProject.BL.Extensions
{
    public static class BusinessLogicLayerServiceExtensions
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // Add DAL services with configuration
            services.AddDataAccessLayerServices(configuration);
            
            // Menambahkan AutoMapper dengan license key
            services.AddAutoMapper(cfg => cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzg1NDU2MDAwIiwiaWF0IjoiMTc1Mzk1MTk4NSIsImFjY291bnRfaWQiOiIwMTk4NWZhZWFmNTU3Y2E2OTE0Y2YxNDlhYjA5M2Y4OSIsImN1c3RvbWVyX2lkIjoiY3RtXzAxazFmdHlyMzNjcDRlMnkweDFjeGZxemJzIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.v63BKrBGiJtijg19XrsTxYaxI-G-gsSwbLllFqUdSiODb3oD-fx1DwxI3UxoMx3lMzFmZW_zyjEVnWiBB3_daD1kghVGXdoKi-kMVenknh7VTDs9QVhf6e-9MjAD6A6mCUaqn0TcI2U-_NB7iiegHUASQDqKnnrhhdUwQcWIJJJrUMGo83kU5MlIT-yKg30xQzCbt9-OBF1olX-c-ocNGVoCVDZj9jQLZNYq6IYR3be6JYBvLYhl7GDYkXniK3daxkNXHaq0EcuroGW_NYDDkCABquUIlQyDJFhLH70jFyTfJE4FR5LkaWjpdriDYQgqeMgaz_CIkthP3fB7Y_F0fg",
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
            services.AddScoped<ICustomerNotificationBL, CustomerNotificationBL>();
            services.AddScoped<IUsmanBL, UsmanBL>();
            services.AddScoped<IPaymentBL, PaymentBL>();
            
            return services;
        }
    }
}