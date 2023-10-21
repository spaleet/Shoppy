﻿using _0_Framework.Application.Extensions;
using _0_Framework.Infrastructure;
using _01_Shoppy.Query.Models.ProductPicture;
using DM.Domain.ProductDiscount;
using IM.Application.Contracts.Inventory.Helpers;
using IM.Domain.Inventory;
using SM.Application.ProductFeature.DTOs;

namespace _01_Shoppy.Query.Helpers.Product;

public class ProductHelper : IProductHelper
{
    #region Ctor

    private readonly IRepository<SM.Domain.Product.Product> _productRepository;
    private readonly IRepository<SM.Domain.ProductCategory.ProductCategory> _productCategoryRepository;
    private readonly IRepository<SM.Domain.ProductPicture.ProductPicture> _productPictureRepository;
    private readonly IRepository<SM.Domain.ProductFeature.ProductFeature> _productFeatureRepository;
    private readonly IRepository<ProductDiscount> _productDiscount;
    private readonly IRepository<Inventory> _inventoryContext;
    private readonly IMapper _mapper;
    private readonly IInventoryHelper _inventoryHelper;

    public ProductHelper(IRepository<SM.Domain.Product.Product> productRepository,
                         IRepository<ProductDiscount> productDiscount,
                         IRepository<Inventory> inventoryContext,
                         IRepository<SM.Domain.ProductPicture.ProductPicture> productPictureRepository,
                         IRepository<SM.Domain.ProductFeature.ProductFeature> productFeatureRepository,
                         IRepository<SM.Domain.ProductCategory.ProductCategory> productCategoryRepository,
                         IMapper mapper,
                         IInventoryHelper inventoryHelper)
    {
        _productRepository = Guard.Against.Null(productRepository, nameof(_productRepository));
        _productCategoryRepository = Guard.Against.Null(productCategoryRepository, nameof(_productCategoryRepository));
        _productPictureRepository = Guard.Against.Null(productPictureRepository, nameof(_productPictureRepository));
        _productFeatureRepository = Guard.Against.Null(productFeatureRepository, nameof(_productFeatureRepository));
        _productDiscount = Guard.Against.Null(productDiscount, nameof(_productDiscount));
        _inventoryContext = Guard.Against.Null(inventoryContext, nameof(_inventoryContext));
        _mapper = Guard.Against.Null(mapper, nameof(_mapper));
        _inventoryHelper = Guard.Against.Null(inventoryHelper, nameof(_inventoryHelper));
    }

    #endregion

    #region MapProductsFromProductCategories

    public async Task<ProductQueryModel> MapProductsFromProductCategories(SM.Domain.Product.Product product)
    {
        var mappedProduct = await MapProducts<ProductQueryModel>(product);

        mappedProduct.CategoryId = product.CategoryId;

        return mappedProduct;
    }

    #endregion

    #region MapProducts

    public async Task<T> MapProducts<T>(SM.Domain.Product.Product req, bool hotDiscountQuery = false) where T : ProductQueryModel
    {
        var product = _mapper.Map(req, default(T));

        #region all discounts query

        var discounts = await _productDiscount.AsQueryable()
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new
            {
                x.ProductId,
                x.Rate
            }).ToListAsyncSafe();

        if (hotDiscountQuery)
        {
            discounts = discounts.Where(x => x.Rate >= 25).ToList();
        }

        #endregion

        product.Category = (await _productCategoryRepository.FindByIdAsync(product.CategoryId)).Title;

        (bool existsProductInventory, decimal productUnitPrice, long currentCount) = GetProductInventory(product.Id).Result;

        if (existsProductInventory)
        {
            // calculate unitPrice
            product.Price = productUnitPrice.ToMoney();
            product.UnitPrice = productUnitPrice;

            if (discounts.Any(x => x.ProductId == product.Id))
            {
                // calculate discountRate
                int discountRate = discounts.FirstOrDefault(x => x.ProductId == product.Id).Rate;
                product.DiscountRate = discountRate;
                product.HasDiscount = discountRate > 0;

                // calculate PriceWithDiscount
                decimal discountAmount = Math.Round((productUnitPrice * discountRate / 100));
                product.PriceWithDiscount = (productUnitPrice - discountAmount).ToMoney();
            }
        }


        return (T)product;
    }

    #endregion

    #region Get Product UnitPrice

    public async Task<(bool, decimal, long)> GetProductInventory(string productId)
    {
        if (!(await _inventoryContext.ExistsAsync(x => x.ProductId == productId)))
            return (false, default, default);

        var filter = Builders<Inventory>.Filter.Eq(x => x.ProductId, productId);
        var inventory = (await _inventoryContext.FindOne(filter));

        long currentCount = await _inventoryHelper.CalculateCurrentCount(inventory.Id);

        return (true, inventory.UnitPrice, currentCount);
    }

    #endregion

    #region GetProductPictures

    public List<ProductPictureQueryModel> GetProductPictures(string productId)
    {
        var productPictures = _productPictureRepository
            .AsQueryable()
            .Where(x => x.ProductId == productId).ToListSafe();

        if (productPictures is null)
            return new List<ProductPictureQueryModel>();

        return productPictures
            .Select(p => _mapper.Map(p, new ProductPictureQueryModel()))
            .ToList();
    }

    #endregion

    #region GetProductFeatures

    public List<ProductFeatureDto> GetProductFeatures(string productId)
    {
        var productFeatures = _productFeatureRepository
            .AsQueryable()
            .Where(x => x.ProductId == productId).ToListSafe();

        if (productFeatures is null)
            return new List<ProductFeatureDto>();

        return productFeatures
            .Select(p => _mapper.Map(p, new ProductFeatureDto()))
            .ToList();
    }

    #endregion

    #region GetProductPriceById

    public decimal GetProductPriceById(string id)
    {
        var inventories = _inventoryContext.AsQueryable()
           .Select(x => new
           {
               x.ProductId,
               x.InStock,
               x.UnitPrice
           }).ToListSafe();

        decimal price = inventories.FirstOrDefault(x => x.ProductId == id).UnitPrice;

        return price;
    }

    #endregion
}