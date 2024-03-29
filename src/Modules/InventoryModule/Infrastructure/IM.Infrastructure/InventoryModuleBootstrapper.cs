﻿using _0_Framework.Infrastructure.IRepository;
using IM.Application.Inventory.Helpers;
using IM.Application.Sevices;
using IM.Application.Inventory.Helpers;
using IM.Domain.Inventory;
using IM.Infrastructure.Seeds;
using IM.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using IM.Infrastructure.AclServices;

namespace IM.Infrastructure;

public class InventoryModuleBootstrapper
{
    public static void Configure(IServiceCollection services, IConfiguration config)
    {
        services.Configure<InventoryDbSettings>(config.GetSection("InventoryDbSettings"));

        services.AddTransient<IRepository<Inventory>, BaseRepository<Inventory, InventoryDbSettings>>();
        services.AddTransient<IRepository<InventoryOperation>, BaseRepository<InventoryOperation, InventoryDbSettings>>();

        services.AddTransient<IInventoryHelper, InventoryHelper>();
        services.AddTransient<IIMProuctAclService, IMProuctAclService>();
        services.AddTransient<IIMAccountAclService, IMAccountAclService>();

        services.AddMediatR(typeof(InventoryModuleBootstrapper).Assembly);

        #region Db Seed

        using var scope = services.BuildServiceProvider().CreateScope();
        var sp = scope.ServiceProvider;

        var logger = sp.GetRequiredService<ILogger<InventoryModuleBootstrapper>>();

        try
        {
            var dbSettings = (InventoryDbSettings)config.GetSection("InventoryDbSettings").Get(typeof(InventoryDbSettings));

            var inventories = InventoryDbSeed.SeedInventories(dbSettings);
            InventoryDbSeed.SeedInventoryOperations(dbSettings, inventories);

            logger.LogInformation("Inventory Module Db Seed Finished Successfully");
        }
        catch (Exception ex)
        {
            logger.LogError("Inventory Module Db Seed Was Unsuccessfull. Execption : {0}", ex.Message);
        }

        #endregion
    }
}