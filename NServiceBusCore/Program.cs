using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NServiceBusCore;

namespace NServiceBus_Core
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddJsonFile("appsettings.json")
                .Build();

            var applicationSettings = new ApplicationSettings();
            config.GetSection("ApplicationSettings").Bind(applicationSettings);

            CreateWebHostBuilder(args, applicationSettings, config).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, ApplicationSettings settings, IConfigurationRoot config) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(settings.ServiceUrl)
                .UseKestrel()
                .UseConfiguration(config);
    }
}
