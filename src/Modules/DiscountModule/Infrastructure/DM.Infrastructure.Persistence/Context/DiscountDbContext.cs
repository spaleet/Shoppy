﻿using _0_Framework.Infrastructure.Context;
using DM.Domain.CustomerDiscount;
using Microsoft.EntityFrameworkCore;

namespace SM.Infrastructure.Persistence.Context;
public class DiscountDbContext : DbContext
{
    #region Ctor

    public DiscountDbContext(DbContextOptions<DiscountDbContext> options) : base(options) { }

    #endregion

    public DbSet<CustomerDiscount> CustomerDiscounts { get; set; }


    #region On Model Creating

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplySharedDbContextOnModelCreatingConfiguration();

        var assembly = typeof(DiscountDbContext).Assembly;
        builder.ApplyConfigurationsFromAssembly(assembly);

        base.OnModelCreating(builder);
    }

    #endregion On Model Creating
}