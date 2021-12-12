﻿using _0_Framework.Domain.IGenericRepository;
using _0_Framework.Infrastructure.GenericRepository;
using IM.Domain.Inventory;
using IM.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IM.Infrastructure.Configuration;

public static class InventoryModuletBootstrapper
{
    public static void Configure(IServiceCollection services, string connectionString)
    {
        services.AddScoped<IGenericRepository<Inventory>, GenericRepository<InventoryDbContext, Inventory>>();

        services.AddMediatR(typeof(InventoryModuletBootstrapper).Assembly);

        services.AddDbContext<InventoryDbContext>(options =>
            options.UseSqlServer(connectionString));
    }
}