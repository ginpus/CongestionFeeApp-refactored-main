using Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;
using System;

namespace CongestionFeeApp
{
    internal class Startup
    {
        public IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IChargeRepository, ChargeRepository>();
            services.AddSingleton<IChargeService, ChargeService>();
            services.AddSingleton<CongestionFeeApp>();

            return services.BuildServiceProvider();
        }
    }
}