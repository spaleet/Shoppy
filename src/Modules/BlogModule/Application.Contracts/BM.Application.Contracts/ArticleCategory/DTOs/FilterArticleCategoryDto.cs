﻿using _0_Framework.Application.Models.Paging;

namespace BM.Application.Contracts.ArticleCategory.DTOs;

public class FilterArticleCategoryDto : BasePaging
{
    #region Properties

    [JsonProperty("Title")]
    [Display(Name = "عنوان")]
    [MaxLength(100, ErrorMessage = DomainErrorMessage.MaxLengthMessage)]
    public string Title { get; set; }

    [JsonProperty("categoryId")]
    [Display(Name = "دسته بندی")]
    public long CategoryId { get; set; }

    [JsonProperty("articleCategories")]
    public IEnumerable<ArticleCategoryDto> ArticleCategories { get; set; }

    #endregion

    #region Methods

    public FilterArticleCategoryDto SetData(IEnumerable<ArticleCategoryDto> articleCategory)
    {
        this.ArticleCategories = articleCategory;
        return this;
    }

    public FilterArticleCategoryDto SetPaging(BasePaging paging)
    {
        this.PageId = paging.PageId;
        this.AllPagesCount = paging.AllPagesCount;
        this.StartPage = paging.StartPage;
        this.EndPage = paging.EndPage;
        this.ShownPages = paging.ShownPages;
        this.SkipPage = paging.SkipPage;
        this.TakePage = paging.TakePage;
        this.PageCount = paging.PageCount;
        return this;
    }

    #endregion
}
