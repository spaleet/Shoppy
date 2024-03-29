﻿using _0_Framework.Application.ErrorMessages;
using Newtonsoft.Json;

namespace _0_Framework.Application.Wrappers;

public class ApiResult<T>
{
    public ApiResult(short status, string message, T data)
    {
        Status = status;
        Message = message;
        Data = data;
    }

    [JsonProperty("status")] public short Status { get; set; }

    [JsonProperty("message")] public string Message { get; set; }

    [JsonProperty("data")] public T Data { get; set; }
}

public class ApiResult
{
    public ApiResult(short status, string message)
    {
        Status = status;
        Message = message;
    }

    [JsonProperty("status")] public short Status { get; set; }

    [JsonProperty("message")] public string Message { get; set; }
}

public static class ApiResponse
{
    #region Success

    public static ApiResult<T> Success<T>(T result, string message = ApplicationErrorMessage.OperationSuccedded)
    {
        return new ApiResult<T>(200, message, result);
    }

    public static ApiResult Success(string message = ApplicationErrorMessage.OperationSuccedded)
    {
        return new ApiResult(200, message);
    }

    #endregion

    #region No Content

    public static ApiResult NoContent()
    {
        return new ApiResult(204, "No Content");
    }

    #endregion

    #region Error

    public static ApiResult Error(string message)
    {
        return new ApiResult(400, message);
    }

    #endregion

    #region Not Found

    public static ApiResult NotFound(string message = ApplicationErrorMessage.RecordNotFound)
    {
        return new ApiResult(404, message);
    }

    #endregion

    #region Unauthorized

    public static ApiResult Unauthorized(string message = ApplicationErrorMessage.Unauthorized)
    {
        return new ApiResult(401, message);
    }

    #endregion

    #region Access Denied

    public static ApiResult AccessDenied(string message = ApplicationErrorMessage.AccessDenied)
    {
        return new ApiResult(403, message);
    }

    #endregion

    #region Client Closed Request

    public static ApiResult ClientClosedRequest()
    {
        // 499 Client Closed Request
        return new ApiResult(499, "Client Closed Request");
    }

    #endregion

    #region Internal Server Error

    public static ApiResult InternalServerError(string msg = "Internal Server Error :(")
    {
        return new ApiResult(500, msg);
    }

    #endregion
}