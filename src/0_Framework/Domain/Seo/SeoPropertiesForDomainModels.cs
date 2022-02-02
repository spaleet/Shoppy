﻿using System.ComponentModel.DataAnnotations;

namespace _0_Framework.Domain.Seo;

public class SeoPropertiesForDomainModels : BaseEntity
{
    [Display(Name = "جزییات تصویر")]
    public string ImageAlt { get; set; }

    [Display(Name = "عنوان تصویر")]
    public string ImageTitle { get; set; }

    [Display(Name = "کلمات کلیدی")]
    public string MetaKeywords { get; set; }

    [Display(Name = "توضیحات Meta")]
    public string MetaDescription { get; set; }
}
