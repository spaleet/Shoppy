﻿namespace Shoppy.WebApi.Endpoints.Admin;

public static class AdminShopApiEndpoints
{
    private const string BaseAdminShopRoute = BaseApiEndpointRoutes.AdminBaseRoute + "/shop";

    public static class ProductCategory
    {
        private const string BaseProductCategoryRoute = BaseAdminShopRoute + "/product-category";

        public const string GetProductCategoriesList = BaseProductCategoryRoute + "/get-list";

        public const string FilterProductCategories = BaseProductCategoryRoute + "/filter";

        public const string GetProductCategoryDetails = BaseProductCategoryRoute + "/{id}";

        public const string CreateProductCategory = BaseProductCategoryRoute + "/create";

        public const string EditProductCategory = BaseProductCategoryRoute + "/edit";

        public const string DeleteProductCategory = BaseProductCategoryRoute + "/delete/{id}";
    }

    public static class Product
    {
        private const string BaseProductRoute = BaseAdminShopRoute + "/product";

        public const string FilterProducts = BaseProductRoute + "/filter";

        public const string GetProductDetails = BaseProductRoute + "/{id}";

        public const string CreateProduct = BaseProductRoute + "/create";

        public const string EditProduct = BaseProductRoute + "/edit";

        public const string DeleteProduct = BaseProductRoute + "/delete/{id}";

        public const string UpdateProductIsInStock = BaseProductRoute + "/update-is-in-stock/{id}";

        public const string UpdateProductNotInStock = BaseProductRoute + "/update-not-in-stock/{id}";
    }

    public static class ProductPicture
    {
        private const string BaseProductPictureRoute = BaseAdminShopRoute + "/product-picture";

        public const string GetProductPictures = BaseProductPictureRoute + "/{productId}";

        public const string CreateProductPicture = BaseProductPictureRoute + "/create";

        public const string RemoveProductPicture = BaseProductPictureRoute + "/remove/{id}";

    }

    public static class Slider
    {
        private const string BaseSliderRoute = BaseAdminShopRoute + "/slider";

        public const string GetSlidersList = BaseSliderRoute + "/get-list";

        public const string GetSliderDetails = BaseSliderRoute + "/{id}";

        public const string CreateSlider = BaseSliderRoute + "/create";

        public const string EditSlider = BaseSliderRoute + "/edit";

        public const string RemoveSlider = BaseSliderRoute + "/remove/{id}";

        public const string RestoreSlider = BaseSliderRoute + "/restore/{id}";
    }
}