using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Formatting.Compact;

namespace WebApplication5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //    Log.Logger = new LoggerConfiguration().WriteTo.File(path: "D:\\serilog-.txt",
            //        outputTemplate:
            //"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
            //        rollingInterval:RollingInterval.Day,rollOnFileSizeLimit:true,retainedFileCountLimit:10).CreateLogger();

            Log.Logger = new LoggerConfiguration()
    .WriteTo.File(new CompactJsonFormatter(), "logs\\log.txt")
    .CreateLogger();

            try
            {
                Log.Information("Application is starting");
                CreateHostBuilder(args).Build().Run();
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, "Application is failed to start");
            }
            finally
            {
                Log.CloseAndFlush();
            }
         
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
