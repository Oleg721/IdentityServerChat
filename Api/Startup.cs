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
using Api.DAL.Repository;
using Api.Model;
using Api.Service;
using Api.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using Api.Mapper;
using Api.Handler;

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

            services.AddAutoMapper(new[] { typeof(MapperDAL), typeof(MapperView) });
            services.AddScoped<IMessageRepository<Message>, MessageRepository>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IUserRepository<ChatUser>, UserRepositiry>();
            services.AddScoped<IUserService<ChatUserDto, string>, UserService>();
            services.AddScoped<ChatUserHandler>();
            services.AddSingleton<OnlineUserService>();
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
                        ValidateIssuer = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/chat")))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
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
