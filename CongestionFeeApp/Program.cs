using Microsoft.Extensions.DependencyInjection;

namespace CongestionFeeApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var startup = new Startup();

            var serviceProvider = startup.ConfigureServices();

            var receipeApp = serviceProvider.GetService<CongestionFeeApp>();

            receipeApp.Start();
        }
    }
}