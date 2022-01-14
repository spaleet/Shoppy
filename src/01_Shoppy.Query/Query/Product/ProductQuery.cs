﻿using _0_Framework.Application.ErrorMessages;
using _0_Framework.Application.Exceptions;
using _0_Framework.Application.Models.Paging;
using _01_Shoppy.Query.Helpers.Product;
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

    #region Search

    public async Task<Response<SearchProductQueryModel>> Search(SearchProductQueryModel search)
    {
        var query = _shopContext.Products
               .OrderByDescending(x => x.LastUpdateDate)
               .AsQueryable();


        #region Filter SelectedCategoriesIds

        if (search.SelectedCategoriesIds is not null && search.SelectedCategoriesIds.Any())
        {
            query = query.Where(x => search.SelectedCategoriesIds.Contains(x.CategoryId.Value));
        }

        #endregion

        #region Filter SelectedCategoriesSlugs

        if (search.SelectedCategoriesSlugs is not null && search.SelectedCategoriesSlugs.Any())
        {
            var selectedCategoriesId = new List<long>();

            foreach (var categorySlug in search.SelectedCategoriesSlugs)
            {
                if (await _shopContext.ProductCategories
                    .AnyAsync(x => x.Slug.Trim() == categorySlug.Trim()))
                {
                    selectedCategoriesId.Add(
                        _shopContext.ProductCategories.FirstOrDefault(x => x.Slug.Trim() == categorySlug.Trim()).Id);
                }
            }

            query = query.Where(x => selectedCategoriesId.Contains(x.CategoryId.Value));
        }

        #endregion

        #region filter

        if (!string.IsNullOrEmpty(search.Phrase))
        {
            query = query.Where(s => EF.Functions.Like(s.Title, $"%{search.Phrase}%")
            || EF.Functions.Like(s.ShortDescription, $"%{search.Phrase}%"));
        }

        switch (search.SortDateOrder)
        {
            case PagingDataSortCreationDateOrder.DES:
                query = query.OrderByDescending(x => x.CreationDate).AsQueryable();
                break;

            case PagingDataSortCreationDateOrder.ASC:
                query = query.OrderBy(x => x.CreationDate).AsQueryable();
                break;
        }

        #endregion filter

        #region paging

        var pager = Pager.Build(search.PageId, await query.CountAsync(),
            search.TakePage, search.ShownPages);
        var allEntities = await query.Paging(pager)
            .AsQueryable()
            .Select(product =>
                   _mapper.Map(product, new ProductQueryModel()))
            .ToListAsync();

        allEntities = await _productHelper.MapProducts(allEntities);

        #endregion paging

        var returnData = search.SetData(allEntities).SetPaging(pager);

        #region Order

        switch (search.SearchProductPriceOrder)
        {
            case SearchProductPriceOrder.Price_Asc:
                returnData.Products = returnData.Products.OrderBy(x => x.UnitPrice);
                break;

            case SearchProductPriceOrder.Price_Des:
                returnData.Products = returnData.Products.OrderByDescending(x => x.UnitPrice);
                break;
        }

        #endregion
        if (returnData.Products is null)
            throw new ApiException(ApplicationErrorMessage.FilteredRecordsNotFoundMessage);

        if (returnData.PageId > returnData.GetLastPage() && returnData.GetLastPage() != 0)
            throw new NotFoundApiException();

        return new Response<SearchProductQueryModel>(returnData);
    }

    #endregion

}
