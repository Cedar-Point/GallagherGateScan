using GallagherGateScan.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GallagherGateScan
{
    public class Program
    {
        public static GallagherAPIService GAPIService = new GallagherAPIService();
        public static void Main(string[] args)
        {
            GAPIService.APIPath = Environment.GetEnvironmentVariable("GGS-APIPath");
            GAPIService.APIKey = Environment.GetEnvironmentVariable("GGS-APIKey");
            GAPIService.APICertThumbprint = Environment.GetEnvironmentVariable("GGS-APICertThumbprint");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
