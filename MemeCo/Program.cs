/**
 * Author:    Jasen Lassig
 * Partner:   Jose Monterroso, Eli Hebdon
 * Date:      December 6, 2019
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Jasen, Jose, Eli - This work may not be copied for use in Academic Coursework.
 *
 * I, Jasen, certify that I wrote this code from scratch and did not copy it in part or whole from 
 * another source.  Any references used in the completion of the assignment are cited in my README file.
 *
 * File Contents
 *
 *    Program file, added database intilization 
 */

using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
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