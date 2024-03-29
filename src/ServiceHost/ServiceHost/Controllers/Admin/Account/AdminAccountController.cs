﻿using AM.Application.Account.Commands;
using AM.Application.Account.DTOs;
using AM.Application.Account.Queries;

namespace ServiceHost.Controllers.Admin.Blog;

[SwaggerTag("مدیریت کاربران")]
public class AdminAccountController : BaseAdminApiController
{
    #region Filter Accounts

    [HttpGet(AdminAccountEndpoints.Account.FilterAccounts)]
    [SwaggerOperation(Summary = "فیلتر  کاربران", Tags = new[] { "AdminAccount" })]
    [SwaggerResponse(200, "success")]
    [SwaggerResponse(400, "error")]
    [ProducesResponseType(typeof(FilterAccountDto), 200)]
    [ProducesResponseType(typeof(ApiResult), 400)]
    public async Task<IActionResult> FilterAccounts([FromQuery] FilterAccountDto filter)
    {
        var res = await Mediator.Send(new FilterAccountsQuery(filter));

        return SuccessResult(res);
    }

    #endregion

    #region Get Account Details

    [HttpGet(AdminAccountEndpoints.Account.GetAccountDetails)]
    [SwaggerOperation(Summary = "دریافت جزییات  کاربر", Tags = new[] { "AdminAccount" })]
    [SwaggerResponse(200, "success")]
    [SwaggerResponse(404, "not-found")]
    [ProducesResponseType(typeof(EditAccountDto), 200)]
    [ProducesResponseType(typeof(ApiResult), 404)]
    public async Task<IActionResult> GetAccountDetails([FromRoute] string id)
    {
        var res = await Mediator.Send(new GetAccountDetailsQuery(id));

        return SuccessResult(res);
    }

    #endregion

    #region Edit Account

    [HttpPut(AdminAccountEndpoints.Account.EditAccount)]
    [SwaggerOperation(Summary = "ویرایش  کاربر", Tags = new[] { "AdminAccount" })]
    [SwaggerResponse(201, "success : created")]
    [SwaggerResponse(400, "error : title is duplicated")]
    [SwaggerResponse(404, "not-found")]
    [ProducesResponseType(typeof(ApiResult), 200)]
    [ProducesResponseType(typeof(ApiResult), 400)]
    [ProducesResponseType(typeof(ApiResult), 404)]
    public async Task<IActionResult> EditAccount([FromForm] EditAccountDto editRequest)
    {
        var res = await Mediator.Send(new EditAccountCommand(editRequest));

        return SuccessResult(res);
    }

    #endregion

    #region Activate Account

    [HttpPost(AdminAccountEndpoints.Account.ActivateAccount)]
    [SwaggerOperation(Summary = "فعال کردن حساب  کاربر", Tags = new[] { "AdminAccount" })]
    [SwaggerResponse(200, "success")]
    [SwaggerResponse(404, "not-found")]
    [ProducesResponseType(typeof(ApiResult), 200)]
    [ProducesResponseType(typeof(ApiResult), 404)]
    public async Task<IActionResult> ActivateAccount([FromRoute] string id)
    {
        var res = await Mediator.Send(new ActivateAccountByAdminCommand(id));

        return SuccessResult(res);
    }

    #endregion

    #region DeActivate Account

    [HttpDelete(AdminAccountEndpoints.Account.DeActivateAccount)]
    [SwaggerOperation(Summary = "غیر فعال کردن حساب  کاربر", Tags = new[] { "AdminAccount" })]
    [SwaggerResponse(200, "success")]
    [SwaggerResponse(404, "not-found")]
    [ProducesResponseType(typeof(ApiResult), 200)]
    [ProducesResponseType(typeof(ApiResult), 404)]
    public async Task<IActionResult> DeActivateAccount([FromRoute] string id)
    {
        var res = await Mediator.Send(new DeActivateAccountCommand(id));

        return SuccessResult(res);
    }

    #endregion
}