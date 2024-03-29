﻿using _0_Framework.Application.Models.Paging;
using _0_Framework.Domain.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using SM.Application.ProductCategory.DTOs;

namespace SM.Application.ProductCategory.Queries;

public record FilterProductCategoriesQuery(FilterProductCategoryDto Filter) : IRequest<FilterProductCategoryDto>;

public class FilterProductCategoriesQueryValidator : AbstractValidator<FilterProductCategoriesQuery>
{
    public FilterProductCategoriesQueryValidator()
    {
        RuleFor(p => p.Filter.Title)
            .MaxLengthValidator("عنوان", 100);
    }
}

public class FilterProductCategoriesQueryHandler : IRequestHandler<FilterProductCategoriesQuery, FilterProductCategoryDto>
{
    private readonly IRepository<Domain.ProductCategory.ProductCategory> _productCategoryRepository;
    private readonly IRepository<Domain.Product.Product> _productRepository;
    private readonly IMapper _mapper;

    public FilterProductCategoriesQueryHandler(IRepository<Domain.ProductCategory.ProductCategory> productCategoryRepository,
        IMapper mapper, IRepository<Domain.Product.Product> productRepository)
    {
        _productCategoryRepository = Guard.Against.Null(productCategoryRepository, nameof(_productCategoryRepository));
        _mapper = Guard.Against.Null(mapper, nameof(_mapper));
        _productRepository = Guard.Against.Null(productRepository, nameof(_productRepository));

    }

    public async Task<FilterProductCategoryDto> Handle(FilterProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _productCategoryRepository.AsQueryable();

        #region filter

        if (!string.IsNullOrEmpty(request.Filter.Title))
            query = query.Where(s => s.Title.Contains(request.Filter.Title));

        switch (request.Filter.SortDateOrder)
        {
            case PagingDataSortCreationDateOrder.DES:
                query = query.OrderByDescending(x => x.CreationDate);
                break;

            case PagingDataSortCreationDateOrder.ASC:
                query = query.OrderBy(x => x.CreationDate);
                break;
        }

        switch (request.Filter.SortIdOrder)
        {
            case PagingDataSortIdOrder.NotSelected:
                break;

            case PagingDataSortIdOrder.DES:
                query = query.OrderByDescending(x => x.Id);
                break;

            case PagingDataSortIdOrder.ASC:
                query = query.OrderBy(x => x.Id);
                break;
        }

        #endregion filter

        #region paging

        var pager = request.Filter.BuildPager((await query.CountAsync()));

        var allEntities =
             _productCategoryRepository
                .ApplyPagination(query, pager)
                .Select(category =>
                    _mapper.Map(category, new ProductCategoryDto
                    {
                        ProductsCount = _productRepository.AsQueryable().Where(x => x.CategoryId == category.Id).Count()
                    }))
                .ToList();

        #endregion paging

        var returnData = request.Filter.SetData(allEntities).SetPaging(pager);

        if (returnData.ProductCategories is null)
            throw new NoContentApiException();

        if (returnData.PageId > returnData.GetLastPage() && returnData.GetLastPage() != 0)
            throw new NoContentApiException();

        return returnData;
    }
}