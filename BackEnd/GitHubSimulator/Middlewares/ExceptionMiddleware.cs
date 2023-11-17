using System.Net;
using GitHubSimulator.Core.BuildingBlocks.Exceptions;
using Newtonsoft.Json;

namespace GitHubSimulator.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            switch (e)
            {
                case ValidationException exception:
                    await ValidationExceptionHandler(context, exception);
                    break;
                default:
                    await UnknownExceptionHandler(context, e);
                    break;
            }
        }
    }

    private static async Task ValidationExceptionHandler(HttpContext context, ValidationException e)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)HttpStatusCode.BadRequest;
        var responseContent = new ResponseContent()
        {
            Error = e.Message,
            Errors = e.Errors
        };
        var jsonResult = JsonConvert.SerializeObject(responseContent);
        await context.Response.WriteAsync(jsonResult);
    }

    private static async Task UnknownExceptionHandler(HttpContext context, Exception e)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var responseContent = new ResponseContent()
        {
            Error = e.Message,
        };
        var jsonResult = JsonConvert.SerializeObject(responseContent);
        await context.Response.WriteAsync(jsonResult);
    }
}
