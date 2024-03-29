﻿using _0_Framework.Application.Extensions;
using AutoMapper;
using SM.Application.Product.DTOs.Admin;
using SM.Application.Product.DTOs.Site;
using SM.Application.ProductCategory.DTOs;
using SM.Application.ProductFeature.DTOs;
using SM.Application.ProductPicture.DTOs;
using SM.Application.Slider.DTOs;
using SM.Domain.Product;
using SM.Domain.ProductCategory;
using SM.Domain.ProductFeature;
using SM.Domain.ProductPicture;
using SM.Domain.Slider;

namespace SM.Infrastructure.Mappings;
public class ShopModuleMappingProfile : Profile
{
    public ShopModuleMappingProfile()
    {
        #region Product Category

        #region Product Category Dto

        CreateMap<ProductCategory, ProductCategoryDto>()
            .ForMember(dest => dest.CreationDate,
                opt => opt.MapFrom(src => src.CreationDate.ToShamsi()));

        #endregion

        #region Create Product Category

        CreateMap<CreateProductCategoryDto, ProductCategory>()
                       .ForMember(dest => dest.Slug,
                           opt => opt.MapFrom(src => src.Title.ToSlug()));

        #endregion

        #region Edit Product Category

        CreateMap<ProductCategory, EditProductCategoryDto>();

        CreateMap<EditProductCategoryDto, ProductCategory>()
            .ForMember(dest => dest.Id,
                opt => opt.Ignore())
            .ForMember(dest => dest.ImagePath,
                opt => opt.Ignore())
            .ForMember(dest => dest.Slug,
                opt => opt.MapFrom(src => src.Title.ToSlug()));

        #endregion

        #region Product Category Query Model

        CreateMap<ProductCategory, SiteProductCategoryDto>();

        #endregion

        #endregion

        #region Product

        #region Product Dto

        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CreationDate,
                opt => opt.MapFrom(src => src.CreationDate.ToShamsi()))
            .ForMember(dest => dest.CategoryTitle,
                opt => opt.MapFrom(src => src.Category.Title));

        #endregion

        #region Create Product

        CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Slug,
                     opt => opt.MapFrom(src => src.Title.ToSlug()))
                .ForMember(dest => dest.Code,
                     opt => opt.MapFrom(src => Generator.Code()));

        #endregion

        #region Edit Product

        CreateMap<Product, EditProductDto>();

        CreateMap<EditProductDto, Product>()
            .ForMember(dest => dest.Id,
                opt => opt.Ignore())
            .ForMember(dest => dest.ImagePath,
                opt => opt.Ignore())
            .ForMember(dest => dest.Code,
                opt => opt.Ignore())
            .ForMember(dest => dest.Slug,
                opt => opt.MapFrom(src => src.Title.ToSlug()));

        #endregion

        #region Product Query Model

        CreateMap<Product, ProductSiteDto>()
            .ForMember(dest => dest.CategorySlug,
                opt => opt.MapFrom(src => src.Category.Slug.ToSlug()));

        #endregion

        #region ProductDetails Query Model

        CreateMap<Product, ProductDetailsSiteDto>()
            .ForMember(dest => dest.CategorySlug,
                opt => opt.MapFrom(src => src.Category.Slug.ToSlug()))
            .ForMember(dest => dest.ProductPictures,
                opt => opt.Ignore());

        #endregion

        #endregion

        #region Product Picture

        #region Product Picture Dto

        CreateMap<ProductPicture, ProductPictureDto>()
            .ForMember(dest => dest.CreationDate,
                opt => opt.MapFrom(src => src.CreationDate.ToShamsi()));

        #endregion

        #region Create Product Picture

        CreateMap<CreateProductPictureDto, ProductPicture>();

        #endregion

        #region ProductPicture Query Model

        CreateMap<ProductPicture, ProductPictureSiteDto>();

        #endregion

        #endregion

        #region Product Feature

        #region Product Feature Dto

        CreateMap<ProductFeature, ProductFeatureDto>()
            .ForMember(dest => dest.CreationDate,
                opt => opt.MapFrom(src => src.CreationDate.ToShamsi()));

        #endregion

        #region Create Product Feature

        CreateMap<CreateProductFeatureDto, ProductFeature>();

        #endregion

        #region Edit Product Feature

        CreateMap<ProductFeature, EditProductFeatureDto>();

        CreateMap<EditProductFeatureDto, ProductFeature>()
            .ForMember(dest => dest.Id,
                opt => opt.Ignore());

        #endregion

        #endregion

        #region Slider

        #region Slider Dto

        CreateMap<Slider, SliderDto>()
            .ForMember(dest => dest.CreationDate,
                opt => opt.MapFrom(src => src.CreationDate.ToShamsi()))
            .ForMember(dest => dest.IsRemoved,
                opt => opt.MapFrom(src => src.IsDeleted));

        #endregion

        #region Create Slider

        CreateMap<CreateSliderDto, Slider>();

        #endregion

        #region Edit Slider

        CreateMap<Slider, EditSliderDto>();

        CreateMap<EditSliderDto, Slider>()
            .ForMember(dest => dest.Id,
                opt => opt.Ignore())
            .ForMember(dest => dest.ImagePath,
                opt => opt.Ignore());

        #endregion

        #region Slider Query Model

        CreateMap<Slider, SiteSliderDto>();

        #endregion

        #endregion

    }
}