﻿using DM.Domain.ProductDiscount;
using MongoDB.Driver;
using SM.Infrastructure.Persistence.Seeds;

namespace DM.Infrastructure.Persistence.Seeds;

public static class DiscountDbSeed
{
    public static void SeedProductDiscounts(IMongoCollection<ProductDiscount> discounts)
    {
        bool existsDiscount = discounts.Find(_ => true).Any();

        if (!existsDiscount)
        {
            ProductDiscount[] inventoryToAdd =
            {
                new ProductDiscount
                {
                    ProductId = SeedProductIdConstants.Product_01,
                    Rate = 25,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(30),
                },
                new ProductDiscount
                {
                    ProductId = SeedProductIdConstants.Product_02,
                    Rate = 10,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(30),
                },
                new ProductDiscount
                {
                    ProductId = SeedProductIdConstants.Product_03,
                    Rate = 75,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(30),
                },
                new ProductDiscount
                {
                    ProductId = SeedProductIdConstants.Product_04,
                    Rate = 30,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(30),
                },
                new ProductDiscount
                {
                    ProductId = SeedProductIdConstants.Product_06,
                    Rate = 53,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(30),
                },
                new ProductDiscount
                {
                    ProductId = SeedProductIdConstants.Product_07,
                    Rate = 99,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(30),
                },
                new ProductDiscount
                {
                    ProductId = SeedProductIdConstants.Product_10,
                   Rate = 10,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(30),
                },
                new ProductDiscount
                {
                    ProductId = SeedProductIdConstants.Product_10,
                    Rate = 10,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(30),
                }
            };
            discounts.InsertManyAsync(inventoryToAdd);
        }
    }

}