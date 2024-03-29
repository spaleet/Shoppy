﻿using FluentValidation;
using SM.Application.ProductCategory.DTOs;
using SM.Application.ProductCategory.Queries;
using System.Collections.Generic;
using System.Linq;

namespace SM.Application.ProductCategory.Queries;
public record GetProductCategoriesListQuery : IRequest<IEnumerable<ProductCategoryForSelectListDto>>;

public class GetProductCategoriesListQueryHandler : IRequestHandler<GetProductCategoriesListQuery, IEnumerable<ProductCategoryForSelectListDto>>
{
    private readonly IRepository<Domain.ProductCategory.ProductCategory> _productCategoryRepository;
    private readonly IMapper _mapper;

    public GetProductCategoriesListQueryHandler(IRepository<Domain.ProductCategory.ProductCategory> productCategoryRepository, IMapper mapper)
    {
        _productCategoryRepository = Guard.Against.Null(productCategoryRepository, nameof(_productCategoryRepository));
        _mapper = Guard.Against.Null(mapper, nameof(_mapper));
    }

    public Task<IEnumerable<ProductCategoryForSelectListDto>> Handle(GetProductCategoriesListQuery request, CancellationToken cancellationToken)
    {
        var categories = _productCategoryRepository
                .AsQueryable()
                .OrderByDescending(p => p.LastUpdateDate)
                .ToList()
                .Select(product => new ProductCategoryForSelectListDto
                {
                    Id = product.Id,
                    Title = product.Title
                });

        NotFoundApiException.ThrowIfNull(categories);

        return Task.FromResult(categories);
    }
}