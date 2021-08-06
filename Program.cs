using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace KingdomApi
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            Log.Information("Starting up!");

            try
            {
                Activity.DefaultIdFormat = ActivityIdFormat.W3C;
                CreateHostBuilder(args).Build().Run();

                // var host = CreateHostBuilder(args).Build();

                // using (var scope = host.Services.CreateScope())
                // {
                //     var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                //     db.Database.Migrate();
                // }

                // host.Run();

                Log.Information("Stopped cleanly");
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An unhandled exception occurred during bootstrapping");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .Enrich.WithClientIp()
                    .Enrich.WithClientAgent()
                    .WriteTo.Console())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
