﻿using _0_Framework.Application.Models.Paging;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CM.Application.Comment.DTOs;

public class FilterCommentDto : BasePaging
{
    #region Properties

    [Display(Name = "نوع")]
    [JsonConverter(typeof(StringEnumConverter))]
    [JsonProperty("type")]
    public FilterCommentType Type { get; set; } = FilterCommentType.All;

    [Display(Name = "وضعیت")]
    [JsonConverter(typeof(StringEnumConverter))]
    [JsonProperty("state")]
    public FilterCommentState State { get; set; } = FilterCommentState.All;

    [Display(Name = "کامنت ها")]
    [JsonProperty("comments")]
    [BindNever]
    public List<CommentDto> Comments { get; set; }

    #endregion

    #region Methods

    public FilterCommentDto SetData(List<CommentDto> comments)
    {
        this.Comments = comments;
        return this;
    }

    public FilterCommentDto SetPaging(BasePaging paging)
    {
        this.PageId = paging.PageId;
        this.DataCount = paging.DataCount;
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


public enum FilterCommentState
{
    [Display(Name = "همه")]
    All,
    [Display(Name = "در حال بررسی")]
    UnderProgress,
    [Display(Name = "رد شده")]
    Canceled,
    [Display(Name = "تایید شده")]
    Confirmed
}

public enum FilterCommentType
{
    [Display(Name = "همه")]
    All,
    [Display(Name = "فروشگاه")]
    Product,
    [Display(Name = "مقالات")]
    Article
}
