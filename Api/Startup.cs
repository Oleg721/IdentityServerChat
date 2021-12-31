// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Api.AddHandler;
using Api.Policy;
using Api.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Claims;
using Api.DAL;
using Api.SignalRHub;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Api.Contracts;
using Api.Repository;
using Api.Model;
using Api.Service;

namespace Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("ChatConnection");

            services.AddScoped<IMessageRepository<Message>, MessageRepository>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddControllers();
            services.AddSignalR();
            //services.AddCors();
            services.AddDbContext<ChatContext>(options => options.UseSqlServer(connectionString));

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder => {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins("http://localhost:4200").SetIsOriginAllowed(e => true);
            }));


            //accepts any access token issued by identity server
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5001";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
            
                });
            services.AddSingleton<ChangePolicyFactory>();
            // adds an authorization policy to make sure the token is for scope 'api1'
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "api1");
                });

                options.AddPolicy("ApiClame", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimTypes.Email, "alice@gmail.com");
                });
                options.AddPolicy("ApiSpacialClaim", policy =>
                {
                    
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(context =>
                    {
                        var u = policy.AuthenticationSchemes;
                        var i = context.User.Claims
                            .Where(e => e.Type == "nbf")
                            .Select(e => e.Value)
                            .FirstOrDefault();
                        Console.WriteLine((int.Parse(i) + 120) > DateTimeOffset.Now.ToUnixTimeSeconds());
                        return (int.Parse(i) + 120 ) > DateTimeOffset.Now.ToUnixTimeSeconds();
                    });
                });

                options.AddPolicy("change", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddRequirements(new ChangeRequirement());
                });

            });
            services.AddSingleton<ChangeClaimsRepository>();
            services.AddSingleton<IAuthorizationHandler, IsMyChangeHandler>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            //app.UseCors(e => e.WithOrigins("http://localhost:4200/chat")
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()
            //    .AllowCredentials()
            //    .SetIsOriginAllowed((host) => {
            //        Console.WriteLine("===============");
            //        Console.WriteLine(host);
            //        return true;
            //    })) ;
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapControllers()
                    .RequireAuthorization("ApiScope");
            });

        }
    }
}
