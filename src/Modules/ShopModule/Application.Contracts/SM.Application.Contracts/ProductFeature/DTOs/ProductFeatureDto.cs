﻿namespace SM.Application.Contracts.ProductFeature.DTOs;

public class ProductFeatureDto
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [Display(Name = "شناسه محصول")]
    [JsonProperty("productId")]
    public long ProductId { get; set; }

    [Display(Name = "عنوان")]
    [JsonProperty("featureTitle")]
    public string FeatureTitle { get; set; }

    [Display(Name = "مقدار")]
    [JsonProperty("featureValue")]
    public string FeatureValue { get; set; }

    [Display(Name = "تاریخ ثبت")]
    [JsonProperty("creationDate")]
    public string CreationDate { get; set; }
}