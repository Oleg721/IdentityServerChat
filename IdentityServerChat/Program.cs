// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using EntityServerTests.Models;
using IdentityServerAspNetIdentity.Data;
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
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

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
                        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                        var signM = services.GetRequiredService<SignInManager<ApplicationUser>>();
                        var context = services.GetRequiredService<ApplicationDbContext>();


                        //var i = context.Clients.Add(new Client()
                        //{
                        //    Id = 10,
                        //    ClientId = "gdgdfdsf",
                        //    ClientName = "testNmae"
                        //});

                        //var i2 = context.Clients.Find(10);

                        // var rolesManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
                        //RoleInitializer.InitializeAsync(userManager, rolesManager).Wait();
                        //userManager.CreateAsync(new ApplicationUser() { UserName = "bob" }, "Pass123$!").Wait();
                        //userManager.CreateAsync(new ApplicationUser() { UserName = "alice" }, "Pass123$").Wait();
                      //  var alice = userManager.FindByNameAsync("alice").Result;
                        ////   var i = userManager.GetRolesAsync(alice).Result;
                        //var r = roleManager.CreateAsync(new IdentityRole() {Name = "adm" }).Result;
                        //var res = userManager.AddClaimsAsync(alice, new List<Claim> { 
                        //    new Claim(ClaimTypes.Email, "alice@gmail.com"),
                        //    new Claim("myOwnClaim", "someClaim")}).Result;
                        //userManager.AddToRoleAsync(alice, "adm");
                       // var i = userManager.AddToRoleAsync(alice, "adm").Result;

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