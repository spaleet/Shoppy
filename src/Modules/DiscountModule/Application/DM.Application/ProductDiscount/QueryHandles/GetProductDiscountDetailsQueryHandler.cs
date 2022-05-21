﻿using DM.Application.Contracts.ProductDiscount.DTOs;
using DM.Application.Contracts.ProductDiscount.Queries;

namespace DM.Application.ProductDiscount.QueryHandles;
public class GetProductDiscountDetailsQueryHandler : IRequestHandler<GetProductDiscountDetailsQuery, EditProductDiscountDto>
{
    #region Ctor

    private readonly IRepository<Domain.ProductDiscount.ProductDiscount> _productDiscountRepository;
    private readonly IMapper _mapper;

    public GetProductDiscountDetailsQueryHandler(IRepository<Domain.ProductDiscount.ProductDiscount> productDiscountRepository, IMapper mapper)
    {
        _productDiscountRepository = Guard.Against.Null(productDiscountRepository, nameof(_productDiscountRepository));
        _mapper = Guard.Against.Null(mapper, nameof(_mapper));
    }

    #endregion

    public async Task<EditProductDiscountDto> Handle(GetProductDiscountDetailsQuery request, CancellationToken cancellationToken)
    {
        var productDiscount = await _productDiscountRepository.FindByIdAsync(request.Id);

        if (productDiscount is null)
            throw new NotFoundApiException();

        return _mapper.Map<EditProductDiscountDto>(productDiscount);
    }
}