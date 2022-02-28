﻿using _0_Framework.Infrastructure.Helpers;
using AM.Application.Contracts.Services;
using AM.Application.Services;
using AM.Domain.Account;
using AM.Domain.Enums;
using AM.Infrastructure.Persistence.Seed;
using AM.Infrastructure.Persistence.Settings;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AM.Infrastructure.Configuration;

public class AccountModuletBootstrapper
{
    public static async Task ConfigureAsync(IServiceCollection services, IConfiguration config)
    {
        #region db config

        services.AddSingleton<AccountDbSettings>(sp =>
        {
            return (AccountDbSettings)config.GetSection("AccountDbSettings").Get(typeof(AccountDbSettings));
        });

        var accountDbSettings = services.BuildServiceProvider().GetRequiredService<AccountDbSettings>();

        services.AddIdentity<Account, AccountRole>()
            .AddMongoDbStores<Account, AccountRole, Guid>
            (
               accountDbSettings.ConnectionString, accountDbSettings.DbName
            );

        services.AddScoped<IGenericRepository<UserToken>, GenericRepository<UserToken, AccountDbSettings>>();

        services.AddMediatR(typeof(AccountModuletBootstrapper).Assembly);

        using (var sp = services.BuildServiceProvider())
        {
            try
            {
                var roleManager = sp.GetRequiredService<RoleManager<AccountRole>>();
                await SeedDefaultRoles.SeedAsync(roleManager);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        services.AddScoped<ITokenFactoryService, TokenFactoryService>();
        services.AddScoped<ITokenStoreService, TokenStoreService>();
        services.AddScoped<ITokenValidatorService, TokenValidatorService>();


        #region auth config

        services.AddSingleton<JwtSettings>(sp =>
        {
            return (JwtSettings)config.GetSection("JwtSettings").Get(typeof(JwtSettings));
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(RoleConstants.Admin, policy => policy.RequireRole(RoleConstants.Admin));
            options.AddPolicy(RoleConstants.BasicUser, policy => policy.RequireRole(RoleConstants.BasicUser));
        });

        var jwtSettings = services.BuildServiceProvider().GetRequiredService<JwtSettings>();

        services
                .AddAuthentication(options =>
                {
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateIssuer = true,
                        ValidAudience = jwtSettings.Audiance,
                        ValidateAudience = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    cfg.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync(ProduceUnAuthorizedResponse(context.Exception.Message));
                        },
                        OnTokenValidated = context =>
                        {
                            var tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<ITokenValidatorService>();
                            return tokenValidatorService.ValidateAsync(context);
                        },
                        OnMessageReceived = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync(ProduceUnAuthorizedResponse());
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync(ProduceUnAuthorizedResponse());
                        }
                    };
                });

        #endregion
    }

    private static string ProduceUnAuthorizedResponse(string message = "لطفا به حساب کاربری خود وارد شوید")
    {
        return JsonConvert.SerializeObject(new
        {
            status = "un-authorized",
            message = message
        });
    }
}
