﻿namespace _01_Shoppy.Query.Contracts.ProductCategory;

public class ProductCategoryQueryModel
{
    [Display(Name = "شناسه")]
    [JsonProperty("id")]
    public long Id { get; set; }

    [Display(Name = "عنوان")]
    [JsonProperty("title")]
    public string Title { get; set; }

    [Display(Name = "تصویر")]
    [JsonProperty("imagePath")]
    public string ImagePath { get; set; }

    [Display(Name = "جزییات تصویر")]
    [JsonProperty("imageAlt")]
    public string ImageAlt { get; set; }

    [Display(Name = "عنوان تصویر")]
    [JsonProperty("imageTitle")]
    public string ImageTitle { get; set; }

    [Display(Name = "کلمات کلیدی")]
    [JsonProperty("metaKeywords")]
    public string MetaKeywords { get; set; }

    [Display(Name = "توضیحات Meta")]
    [JsonProperty("metaDescription")]
    public string MetaDescription { get; set; }

    [Display(Name = "عنوان لینک")]
    [JsonProperty("slug")]
    public string Slug { get; set; }

    [Display(Name = "محصولات")]
    [JsonProperty("products")]
    public List<ProductQueryModel> Products { get; set; } = new List<ProductQueryModel>();
}
