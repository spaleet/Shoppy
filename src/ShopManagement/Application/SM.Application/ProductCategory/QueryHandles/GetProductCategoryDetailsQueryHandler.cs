﻿using System.Threading;
using System.Threading.Tasks;
using _0_Framework.Application.Exceptions;
using _0_Framework.Application.Wrappers;
using _0_Framework.Domain.IGenericRepository;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using SM.Application.Contracts.ProductCategory.Models;
using SM.Application.Contracts.ProductCategory.Queries;

namespace SM.Application.ProductCategory.QueryHandles
{
    public class GetProductCategoryDetailsQueryHandler : IRequestHandler<GetProductCategoryDetailsQuery, Response<EditProductCategoryDto>>
    {
        #region Ctor

        private readonly IGenericRepository<Domain.ProductCategory.ProductCategory> _productCategoryRepository;
        private readonly IMapper _mapper;

        public GetProductCategoryDetailsQueryHandler(IGenericRepository<Domain.ProductCategory.ProductCategory> productCategoryRepository, IMapper mapper)
        {
            _productCategoryRepository = Guard.Against.Null(productCategoryRepository, nameof(_productCategoryRepository));
            _mapper = Guard.Against.Null(mapper, nameof(_mapper));
        }

        #endregion

        public async Task<Response<EditProductCategoryDto>> Handle(GetProductCategoryDetailsQuery request, CancellationToken cancellationToken)
        {
            var productCategory = await _productCategoryRepository.GetEntityById(request.Id);

            if (productCategory is null)
                throw new NotFoundApiException();

            var mappedProductCategory = _mapper.Map<EditProductCategoryDto>(productCategory);

            return new Response<EditProductCategoryDto>(mappedProductCategory);
        }
    }
}