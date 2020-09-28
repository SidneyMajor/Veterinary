using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Veterinary.Data;

namespace Veterinary
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Para receber o seed

            var host = CreateWebHostBuilder(args).Build();

            RunSeeding(host);
            host.Run();

            //CreateWebHostBuilder(args).Build().Run();
        }

        private static void RunSeeding(IWebHost host)
        {
            //uso de factory
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<SeedDb>();
                seeder.SeedAsync().Wait();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
