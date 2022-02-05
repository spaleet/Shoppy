﻿using _0_Framework.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace _02_DI_Container.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var status = "error";
            var errors = new List<string>();
            var errorMessage = error?.Message;

            errors.Add(error.Message);

            switch (error)
            {
                case ApiException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case NotFoundApiException e:
                    // custom not-found application error
                    status = "not-found";
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case NoContentApiException e:
                    // custom no-content application error
                    status = "no-content";
                    response.StatusCode = (int)HttpStatusCode.NoContent;
                    break;

                case ValidationException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        errors.Add(e.Errors[i]);
                    }
                    break;

                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonConvert.SerializeObject(new
            {
                status = status,
                message = errorMessage,
                errors = errors
            }, Formatting.Indented);

            await response.WriteAsync(result);
        }
    }
}