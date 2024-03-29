﻿using CM.Application.Comment.Commands;
using CM.Application.Comment.DTOs;
using CM.Application.Comment.Queries;

namespace ServiceHost.Controllers.Admin.Comment;

[SwaggerTag("مدیریت کامنت ها")]
public class AdminCommentController : BaseAdminApiController
{
    #region Filter Comment

    [HttpGet(AdminCommentEndpoints.Comment.FilterComments)]
    [SwaggerOperation(Summary = "فیلتر کامنت ها", Tags = new[] { "AdminComment" })]
    [SwaggerResponse(200, "success")]
    [SwaggerResponse(404, "not-found")]
    [ProducesResponseType(typeof(FilterCommentDto), 200)]
    [ProducesResponseType(typeof(ApiResult), 404)]
    public async Task<IActionResult> FilterComments([FromQuery] FilterCommentDto filter,
        CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(new FilterCommentsQuery(filter), cancellationToken);

        return SuccessResult(res);
    }

    #endregion

    #region Confirm Comment

    [HttpPost(AdminCommentEndpoints.Comment.ConfirmComment)]
    [SwaggerOperation(Summary = "تایید کامنت", Tags = new[] { "AdminComment" })]
    [SwaggerResponse(200, "success")]
    [SwaggerResponse(404, "not-found")]
    [ProducesResponseType(typeof(ApiResult), 200)]
    [ProducesResponseType(typeof(ApiResult), 404)]
    public async Task<IActionResult> ConfirmComment([FromRoute] string id)
    {
        var res = await Mediator.Send(new ConfirmCommentCommand(id));

        return SuccessResult(res);
    }

    #endregion

    #region Cancel Comment

    [HttpPost(AdminCommentEndpoints.Comment.CancelComment)]
    [SwaggerOperation(Summary = "حذف کامنت", Tags = new[] { "AdminComment" })]
    [SwaggerResponse(200, "success")]
    [SwaggerResponse(404, "not-found")]
    [ProducesResponseType(typeof(ApiResult), 200)]
    [ProducesResponseType(typeof(ApiResult), 404)]
    public async Task<IActionResult> CancelComment([FromRoute] string id)
    {
        var res = await Mediator.Send(new CancelCommentCommand(id));

        return SuccessResult(res);
    }

    #endregion
}