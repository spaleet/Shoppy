﻿using DM.Application.Contracts.ProductDiscount.Commands;
using DM.Application.Contracts.ProductDiscount.DTOs;
using DM.Application.Contracts.ProductDiscount.Queries;

namespace Shoppy.WebApi.Controllers.Admin.Discount;

[SwaggerTag("مدیریت تخفیفات محصول")]
public class AdminProductDiscountController : BaseAdminApiController
{
    #region Filter Customer Discounts

    [HttpGet(AdminDiscountEndpoints.ProductDiscount.FilterProductDiscounts)]
    [SwaggerOperation(Summary = "فیلتر تخفیفات محصول", Tags = new[] { "AdminProductDiscount" })]
    [SwaggerResponse(200, "success")]
    public async Task<IActionResult> FilterProductDiscounts([FromQuery] FilterProductDiscountDto filter)
    {
        var res = await Mediator.Send(new FilterProductDiscountsQuery(filter));

        return JsonApiResult.Success(res);
    }

    #endregion

    #region Get ProductDiscount Details

    [HttpGet(AdminDiscountEndpoints.ProductDiscount.GetProductDiscountDetails)]
    [SwaggerOperation(Summary = "دریافت تخفیف محصول", Tags = new[] { "AdminProductDiscount" })]
    [SwaggerResponse(200, "success")]
    [SwaggerResponse(404, "not-found")]
    public async Task<IActionResult> GetProductDiscountDetails([FromRoute] string id)
    {
        var res = await Mediator.Send(new GetProductDiscountDetailsQuery(id));

        return JsonApiResult.Success(res);
    }

    #endregion

    #region Define Customer Discount

    [HttpPost(AdminDiscountEndpoints.ProductDiscount.DefineProductDiscount)]
    [SwaggerOperation(Summary = "تعریف تخفیف محصول", Tags = new[] { "AdminProductDiscount" })]
    [SwaggerResponse(201, "success : created")]
    [SwaggerResponse(400, "error : discount exists for product")]
    [SwaggerResponse(404, "not-found")]
    public async Task<IActionResult> DefineProductDiscount([FromForm] DefineProductDiscountDto createRequest)
    {
        var res = await Mediator.Send(new DefineProductDiscountCommand(createRequest));

        return JsonApiResult.Created(res);
    }

    #endregion

    #region Edit Customer Discount

    [HttpPut(AdminDiscountEndpoints.ProductDiscount.EditProductDiscount)]
    [SwaggerOperation(Summary = "ویرایش تخفیف محصول", Tags = new[] { "AdminProductDiscount" })]
    [SwaggerResponse(200, "success")]
    [SwaggerResponse(404, "not-found")]
    public async Task<IActionResult> EditProductDiscount([FromForm] EditProductDiscountDto editRequest)
    {
        var res = await Mediator.Send(new EditProductDiscountCommand(editRequest));

        return JsonApiResult.Success(res);
    }

    #endregion

    #region Remove Customer Discount

    [HttpDelete(AdminDiscountEndpoints.ProductDiscount.RemoveProductDiscount)]
    [SwaggerOperation(Summary = "حذف تخفیف محصول", Tags = new[] { "AdminProductDiscount" })]
    [SwaggerResponse(200, "success")]
    [SwaggerResponse(404, "not-found")]
    public async Task<IActionResult> RemoveProductDiscount([FromRoute] string id)
    {
        var res = await Mediator.Send(new RemoveProductDiscountCommand(id));

        return JsonApiResult.Success(res);
    }

    #endregion

    #region Check Product Has Customer Discount

    [HttpGet(AdminDiscountEndpoints.ProductDiscount.CheckProductHasProductDiscount)]
    [SwaggerOperation(Summary = "چک کردن وجود تخفیف محصول برای محصول", Tags = new[] { "AdminProductDiscount" })]
    [SwaggerResponse(200, "success")]
    [SwaggerResponse(404, "not-found")]
    public async Task<IActionResult> CheckProductHasProductDiscount([FromRoute] string productId)
    {
        var res = await Mediator.Send(new CheckProductHasProductDiscountQuery(productId));

        return JsonApiResult.Success(res);
    }

    #endregion
}
