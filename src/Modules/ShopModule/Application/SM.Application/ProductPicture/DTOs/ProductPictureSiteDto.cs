﻿namespace SM.Application.ProductPicture.DTOs;

public class ProductPictureSiteDto
{
    [Display(Name = "شناسه")]
    [JsonProperty("id")]
    public string Id { get; set; }

    [Display(Name = "تصویر")]
    [JsonProperty("imagePath")]
    public string ImagePath { get; set; }
}
