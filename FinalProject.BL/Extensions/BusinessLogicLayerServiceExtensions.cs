using FinalProject.BL.BL;
using FinalProject.BL.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FinalProject.BL.Extensions
{
    public static class BusinessLogicLayerServiceExtensions
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
        {
            services.AddScoped<ISalesAgreementBL, SalesAgreementBL>();
            services.AddScoped<ILetterOfIntentBL, LetterOfIntentBL>();
            services.AddScoped<IDealerInventoryBL, DealerInventoryBL>();
            services.AddScoped<ISalesPersonBL, SalesPersonBL>();
            services.AddScoped<ICarBL, CarBL>();
            services.AddScoped<ICustomerBL, CustomerBL>();
            services.AddScoped<IDealerBL, DealerBL>();
            return services;
        }
    }
}