using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using MemeCo.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using MemeCo.Models;
using MemeCo;

namespace Learning_Outcome_Tracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            var services = host.Services.CreateScope().ServiceProvider;
            try
            {
                var userManager = services.GetRequiredService<UserManager<MemeCoUser>>();
                var _memeCoContext = services.GetRequiredService<MemeCoContext>();
                DbInitializer initializer = new DbInitializer(userManager, _memeCoContext);
                initializer.Seed();

            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }


            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
