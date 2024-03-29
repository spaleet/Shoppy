﻿using _0_Framework.Application.Models.Paging;
using IM.Application.Inventory.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace IM.Application.Inventory.DTOs;

public class FilterInventoryDto : BasePaging
{
    #region Properties

    [Display(Name = "عنوان محصول")]
    [JsonProperty("productTitle")]
    public string ProductTitle { get; set; }

    [Display(Name = "وضعیت")]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    [JsonProperty("inStockState")]
    public FilterInventoryInStockStateEnum InStockState { get; set; } = FilterInventoryInStockStateEnum.All;

    [Display(Name = "انبار ها")]
    [BindNever]
    [JsonProperty("inventories")]
    public List<InventoryDto> Inventories { get; set; }

    #endregion

    #region Methods

    public FilterInventoryDto SetData(List<InventoryDto> inventories)
    {
        this.Inventories = inventories;
        return this;
    }

    public FilterInventoryDto SetPaging(BasePaging paging)
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
