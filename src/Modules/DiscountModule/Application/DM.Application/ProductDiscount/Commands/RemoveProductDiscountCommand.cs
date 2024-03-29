﻿using FluentValidation;

namespace DM.Application.ProductDiscount.CommandHandles;

public record RemoveProductDiscountCommand(string ProductDiscountId) : IRequest<ApiResult>;

public class RemoveProductDiscountCommandValidator : AbstractValidator<RemoveProductDiscountCommand>
{
    public RemoveProductDiscountCommandValidator()
    {
        RuleFor(p => p.ProductDiscountId)
            .RequiredValidator("شناسه تخفیف");
    }
}

public class RemoveProductDiscountCommandHandler : IRequestHandler<RemoveProductDiscountCommand, ApiResult>
{
    private readonly IRepository<Domain.ProductDiscount.ProductDiscount> _productDiscountRepository;

    public RemoveProductDiscountCommandHandler(IRepository<Domain.ProductDiscount.ProductDiscount> productDiscountRepository)
    {
        _productDiscountRepository = Guard.Against.Null(productDiscountRepository, nameof(_productDiscountRepository));
    }

    public async Task<ApiResult> Handle(RemoveProductDiscountCommand request, CancellationToken cancellationToken)
    {
        var productDiscount = await _productDiscountRepository.FindByIdAsync(request.ProductDiscountId);

        NotFoundApiException.ThrowIfNull(productDiscount);

        await _productDiscountRepository.DeletePermanentAsync(productDiscount.Id);

        return ApiResponse.Success(ApplicationErrorMessage.RecordDeleted);
    }
}