﻿using _01_Shoppy.Query.Helpers.Product;
using AutoMapper;
using DM.Infrastructure.Persistence.Context;
using SM.Infrastructure.Persistence.Context;

namespace _01_Shoppy.Query.Query;

public class ProductQuery : IProductQuery
{
    #region Ctor

    private readonly ShopDbContext _shopContext;
    private readonly DiscountDbContext _discountContext;
    private readonly IProductHelper _productHelper;
    private readonly IMapper _mapper;

    public ProductQuery(
        ShopDbContext shopContext, DiscountDbContext discountContext,
        IProductHelper productHelper, IMapper mapper)
    {
        _shopContext = Guard.Against.Null(shopContext, nameof(_shopContext));
        _discountContext = Guard.Against.Null(discountContext, nameof(_discountContext));
        _productHelper = Guard.Against.Null(productHelper, nameof(_productHelper));
        _mapper = Guard.Against.Null(mapper, nameof(_mapper));
    }

    #endregion

    #region Get Latest Products

    public async Task<Response<List<ProductQueryModel>>> GetLatestProducts()
    {
        var latestProducts = await _shopContext.Products
               .Include(x => x.Category)
               .OrderByDescending(x => x.LastUpdateDate)
               .Take(8)
               .Select(product =>
                   _mapper.Map(product, new ProductQueryModel()))
               .ToListAsync();

        var returnData = await _productHelper.MapProducts(latestProducts);

        return new Response<List<ProductQueryModel>>(returnData);
    }

    #endregion

    #region Get Hotest Discount Products

    public async Task<Response<List<ProductQueryModel>>> GetHotestDiscountProducts()
    {
        List<long> hotDiscountRateIds = await _discountContext.CustomerDiscounts.AsQueryable()
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Where(x => x.Rate >= 25)
            .Take(8)
            .Select(x => x.ProductId).ToListAsync();

        var products = await _shopContext.Products
               .Include(x => x.Category)
               .Where(x => hotDiscountRateIds.Contains(x.Id))
               .OrderByDescending(x => x.LastUpdateDate)
               .Take(8)
               .Select(product =>
                   _mapper.Map(product, new ProductQueryModel()))
               .ToListAsync();

        var returnData = await _productHelper.MapProducts(products, true);

        return new Response<List<ProductQueryModel>>(returnData);
    }

    #endregion
}