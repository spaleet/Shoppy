﻿using _0_Framework.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using SM.Domain.ProductCategory;
using SM.Infrastructure.Persistence.Mappings;

namespace SM.Infrastructure.Persistence.Context
{
    public class ShopDbContext : DbContext
    {
        #region Ctor

        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options) { }

        #endregion

        public DbSet<ProductCategory> ProductCategories { get; set; }

        #region On Model Creating

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplySharedDbContextOnModelCreatingConfiguration();

            var assembly = typeof(ShopDbContext).Assembly;
            builder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(builder);
        }

        #endregion On Model Creating
    }
}