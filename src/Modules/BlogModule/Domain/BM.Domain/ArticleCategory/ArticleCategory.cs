﻿using _0_Framework.Domain.Seo;
using System.ComponentModel.DataAnnotations;

namespace BM.Domain.ArticleCategory;

public class ArticleCategory : SeoPropertiesForDomainModels
{
    #region Properties

    [Display(Name = "عنوان")]
    public string Title { get; set; }

    [Display(Name = "توضیحات")]
    public string Description { get; set; }

    [Display(Name = "ترتیب نمایش")]
    public int OrderShow { get; set; }

    [Display(Name = "تصویر")]
    public string ImagePath { get; set; }

    [Display(Name = "عنوان لینک")]
    public string Slug { get; set; }

    [Display(Name = "عنوان لینک")]
    public string CanonicalAddress { get; set; }

    #endregion

    #region Relations

    public virtual ICollection<Article.Article> Articles { get; set; }

    #endregion
}
