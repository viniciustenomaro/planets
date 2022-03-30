using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace TesteApi.WebAPI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var directory = Directory.GetCurrentDirectory();
            CreateHostBuilder(args, directory).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, string directory) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();                    
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseContentRoot(directory);
                    webBuilder.UseWebRoot(Path.Combine(directory, "wwwroot"));
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel();
                });
    }
}
