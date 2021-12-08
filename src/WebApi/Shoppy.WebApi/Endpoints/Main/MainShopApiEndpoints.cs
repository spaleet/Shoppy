﻿namespace Shoppy.WebApi.Endpoints.Main;

public static class MainShopApiEndpoints
{
    private const string BaseMainShopRoute = BaseApiEndpointRoutes.BaseRoute + "/shop";

    public static class Slider
    {
        private const string BaseSliderRoute = BaseMainShopRoute + "/slider";

        public const string GetSliders = BaseSliderRoute + "/get-list";
    }

    public static class ProductCategory
    {
        private const string BaseProductCategoryRoute = BaseMainShopRoute + "/product-category";

        public const string GetProductCategories = BaseProductCategoryRoute + "/get-list";
    }
}