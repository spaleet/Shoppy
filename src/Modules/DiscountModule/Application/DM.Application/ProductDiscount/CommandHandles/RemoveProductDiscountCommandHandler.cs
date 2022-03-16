﻿using DM.Application.Contracts.ProductDiscount.Commands;

namespace DM.Application.ProductDiscount.CommandHandles;

public class RemoveProductDiscountCommandHandler : IRequestHandler<RemoveProductDiscountCommand, Response<string>>
{
    #region Ctor

    private readonly IRepository<Domain.ProductDiscount.ProductDiscount> _productDiscountRepository;

    public RemoveProductDiscountCommandHandler(IRepository<Domain.ProductDiscount.ProductDiscount> productDiscountRepository)
    {
        _productDiscountRepository = Guard.Against.Null(productDiscountRepository, nameof(_productDiscountRepository));
    }

    #endregion

    public async Task<Response<string>> Handle(RemoveProductDiscountCommand request, CancellationToken cancellationToken)
    {
        var productDiscount = await _productDiscountRepository.GetByIdAsync(request.ProductDiscountId);

        if (productDiscount is null)
            throw new NotFoundApiException();

        await _productDiscountRepository.DeletePermanentAsync(productDiscount.Id);

        return new Response<string>(ApplicationErrorMessage.RecordDeleted);
    }
}