﻿using Microsoft.AspNetCore.Mvc;
using Shoppy.Admin.WebApi.Endpoints;
using SM.Application.Contracts.Product.Commands;
using SM.Application.Contracts.Product.DTOs;
using SM.Application.Contracts.Product.Queries;

namespace Shoppy.Admin.WebApi.Controllers;

public class ProductController : BaseApiController
{
    /// <summary>
    ///    فیلتر محصولات
    /// </summary>
    /// <response code="200">Success</response>
    [HttpGet(ApiEndpoints.Product.FilterProducts)]
    public async Task<IActionResult> FilterProducts([FromQuery] FilterProductDto filter)
    {
        var res = await Mediator.Send(new FilterProductsQuery(filter));

        return JsonApiResult.Success(res);
    }

    /// <summary>
    ///    دریافت جزییات محصول
    /// </summary>
    /// <response code="200">Success</response>
    [HttpGet(ApiEndpoints.Product.GetProductDetails)]
    public async Task<IActionResult> GetProductDetails([FromRoute] long id)
    {
        var res = await Mediator.Send(new GetProductDetailsQuery(id));

        return JsonApiResult.Success(res);
    }

    /// <summary>
    ///    ایجاد محصول
    /// </summary>
    /// <response code="200">Success</response>
    [HttpPost(ApiEndpoints.Product.CreateProduct)]
    public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto createRequest)
    {
        var res = await Mediator.Send(new CreateProductCommand(createRequest));

        return JsonApiResult.Success(res);
    }

    /// <summary>
    ///    ویرایش محصول
    /// </summary>
    /// <response code="200">Success</response>
    [HttpPut(ApiEndpoints.Product.EditProduct)]
    public async Task<IActionResult> EditProduct([FromForm] EditProductDto editRequest)
    {
        var res = await Mediator.Send(new EditProductCommand(editRequest));

        return JsonApiResult.Success(res);
    }

    /// <summary>
    ///    حذف محصول
    /// </summary>
    /// <response code="200">Success</response>
    [HttpDelete(ApiEndpoints.Product.DeleteProduct)]
    public async Task<IActionResult> DeleteProduct([FromRoute] long id)
    {
        var res = await Mediator.Send(new DeleteProductCommand(id));

        return JsonApiResult.Success(res);
    }

    /// <summary>
    ///    ثبت موجودی محصول
    /// </summary>
    /// <response code="200">Success</response>
    [HttpPut(ApiEndpoints.Product.UpdateProductIsInStock)]
    public async Task<IActionResult> UpdateProductIsInStock([FromRoute] long id)
    {
        var res = await Mediator.Send(new UpdateProductIsInStockCommand(id));

        return JsonApiResult.Success(res);
    }

    /// <summary>
    ///    ثبت نا موجودی محصول
    /// </summary>
    /// <response code="200">Success</response>
    [HttpDelete(ApiEndpoints.Product.UpdateProductNotInStock)]
    public async Task<IActionResult> UpdateProductNotInStock([FromRoute] long id)
    {
        var res = await Mediator.Send(new UpdateProductNotInStockCommand(id));

        return JsonApiResult.Success(res);
    }
}
