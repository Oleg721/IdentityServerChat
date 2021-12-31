// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Linq;

namespace IdentityServerAspNetIdentity
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                // uncomment to write to Azure diagnostics stream
                //.WriteTo.File(
                //    @"D:\home\LogFiles\Application\identityserver.txt",
                //    fileSizeLimitBytes: 1_000_000,
                //    rollOnFileSizeLimit: true,
                //    shared: true,
                //    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                .CreateLogger();
            
            try
            {
                var seed = args.Contains("/seed");
                if (seed)
                {
                    args = args.Except(new[] { "/seed" }).ToArray();
                }

                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                        var signM = services.GetRequiredService<SignInManager<ApplicationUser>>();
                        // var rolesManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
                        //RoleInitializer.InitializeAsync(userManager, rolesManager).Wait();
                      //  userManager.CreateAsync(new ApplicationUser() { UserName = "alice" }, "Pass123$").Wait();
                        var alice = userManager.FindByNameAsync("alice").Result;

                        //var r = signM.PasswordSignInAsync(alice, "Pass123$",false,false).Result;
                        //var r = userManager.CheckPasswordAsync(alice, "Pass123$").Result;
                        var rr = signM.CheckPasswordSignInAsync(alice, "Pass123$", false).Result;

                    }
                    catch (Exception ex)
                    {
                       Console.WriteLine( "An error occurred while seeding the database.");
                    }
                }
                //if (seed)
                //{
                //    Log.Information("Seeding database...");
                //    var config = host.Services.GetRequiredService<IConfiguration>();
                //    var connectionString = config.GetConnectionString("DefaultConnection");
                //    SeedData.EnsureSeedData(connectionString);
                //    Log.Information("Done seeding database.");
                //    return 0;
                //}

                Log.Information("Starting host...");
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
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