﻿using SM.Application.Contracts.Product.Commands;
using SM.Application.Contracts.Product.DTOs;

namespace SM.Application.Product.CommandHandles;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApiResult<CreateProductResponseDto>>
{
    #region Ctor

    private readonly IRepository<Domain.Product.Product> _productRepository;
    private readonly IRepository<Domain.ProductPicture.ProductPicture> _productPictureRepository;

    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IRepository<Domain.Product.Product> productRepository,
            IRepository<Domain.ProductPicture.ProductPicture> productPictureRepository, IMapper mapper)
    {
        _productRepository = Guard.Against.Null(productRepository, nameof(_productRepository));
        _productPictureRepository = Guard.Against.Null(productPictureRepository, nameof(_productPictureRepository));
        _mapper = Guard.Against.Null(mapper, nameof(_mapper));
    }

    #endregion

    public async Task<ApiResult<CreateProductResponseDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (await _productRepository.ExistsAsync(x => x.Title == request.Product.Title))
            throw new ApiException(ApplicationErrorMessage.DuplicatedRecordExists);

        var product =
            _mapper.Map(request.Product, new Domain.Product.Product());

        string imagePath = request.Product.ImageFile.GenerateImagePath();

        request.Product.ImageFile.AddImageToServer(imagePath, PathExtension.ProductImage, 200, 200, PathExtension.ProductThumbnailImage);
        product.ImagePath = imagePath;

        await _productRepository.InsertAsync(product);

        request.Product.ImageFile.AddImageToServer(imagePath, PathExtension.ProductPictureImage, 150, 150, PathExtension.ProductPictureThumbnailImage);

        await _productPictureRepository.InsertAsync(new Domain.ProductPicture.ProductPicture
        {
            ProductId = product.Id,
            ImagePath = imagePath
        });

        return ApiResponse.Success<CreateProductResponseDto>(new CreateProductResponseDto(product.Id));
    }
}