﻿using _0_Framework.Domain;
using _0_Framework.Domain.Attributes;
using _0_Framework.Domain.Seo;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BM.Domain.ArticleCategory;

[BsonCollection("articleCategories")]
public class ArticleCategory : SeoPropertiesForDomainModels
{
    #region Properties

    [Display(Name = "عنوان")]
    [BsonElement("title")]
    [Required(ErrorMessage = DomainErrorMessage.Required)]
    [MaxLength(100, ErrorMessage = DomainErrorMessage.MaxLength)]
    public string Title { get; set; }

    [Display(Name = "توضیحات")]
    [BsonElement("description")]
    [Required(ErrorMessage = DomainErrorMessage.Required)]
    [MaxLength(250, ErrorMessage = DomainErrorMessage.MaxLength)]
    public string Description { get; set; }

    [Display(Name = "ترتیب نمایش")]
    [BsonElement("orderShow")]
    [Range(1, 1000, ErrorMessage = DomainErrorMessage.Required)]
    public int OrderShow { get; set; }

    [Display(Name = "تصویر")]
    [BsonElement("imagePath")]
    [Required(ErrorMessage = DomainErrorMessage.Required)]
    public string ImagePath { get; set; }

    [Display(Name = "عنوان لینک")]
    [BsonElement("slug")]
    [Required(ErrorMessage = DomainErrorMessage.Required)]
    public string Slug { get; set; }

    [Display(Name = "عنوان لینک")]
    [BsonElement("canonicalAddress")]
    public string CanonicalAddress { get; set; }

    #endregion
}
