﻿using SM.Application.Contracts.ProductCategory.Commands;

namespace SM.Application.ProductCategory.CommandHandles;

public class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand, ApiResult>
{
    #region Ctor

    private readonly IRepository<Domain.ProductCategory.ProductCategory> _productCategoryRepository;
    private readonly IMapper _mapper;

    public CreateProductCategoryCommandHandler(IRepository<Domain.ProductCategory.ProductCategory> productCategoryRepository, IMapper mapper)
    {
        _productCategoryRepository = Guard.Against.Null(productCategoryRepository, nameof(_productCategoryRepository));
        _mapper = Guard.Against.Null(mapper, nameof(_mapper));
    }

    #endregion

    public async Task<ApiResult> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        if (await _productCategoryRepository.ExistsAsync(x => x.Title == request.ProductCategory.Title))
            throw new ApiException(ApplicationErrorMessage.DuplicatedRecordExists);

        var productCategory =
            _mapper.Map(request.ProductCategory, new Domain.ProductCategory.ProductCategory());

        string imagePath = request.ProductCategory.ImageFile.GenerateImagePath();

        request.ProductCategory.ImageFile.AddImageToServer(imagePath, PathExtension.ProductCategoryImage,
                    200, 200, PathExtension.ProductCategoryThumbnailImage);

        productCategory.ImagePath = imagePath;

        await _productCategoryRepository.InsertAsync(productCategory);

        return ApiResponse.Success();
    }
}