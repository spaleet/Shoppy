﻿using _0_Framework.Application.Models.Paging;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
namespace DM.Application.DiscountCode.DTOs;

public class FilterDiscountCodeDto : BasePaging
{
    #region Properties

    [JsonProperty("phrase")]
    public string Phrase { get; set; }

    [JsonProperty("discounts")]
    [BindNever]
    public List<DiscountCodeDto> Discounts { get; set; }

    #endregion

    #region Methods

    public FilterDiscountCodeDto SetData(List<DiscountCodeDto> discounts)
    {
        this.Discounts = discounts;
        return this;
    }

    public FilterDiscountCodeDto SetPaging(BasePaging paging)
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