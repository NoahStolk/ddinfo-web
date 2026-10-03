using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Middleware;

internal sealed class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await next(context);
		}
		catch (StatusCodeException ex)
		{
			if (context.Response.HasStarted)
			{
				logger.LogWarning(ex, "The response has already started, the exception middleware will not be executed.");
				throw;
			}

			context.Response.Clear();
			context.Response.StatusCode = (int)ex.StatusCode;

			// WriteAsJsonAsync overwrites the content type with application/json unless it is passed explicitly.
			ProblemDetails problemDetails = new()
			{
				Status = (int)ex.StatusCode,
				Title = ex.ExposeInnerExceptionMessages ? DisplayException(ex) : ex.Message,
			};
			await context.Response.WriteAsJsonAsync(problemDetails, options: null, contentType: "application/problem+json; charset=utf-8");
		}

		static string DisplayException(Exception ex)
		{
			if (ex.InnerException == null)
				return ex.Message;

			return ex.Message + Environment.NewLine + DisplayException(ex.InnerException);
		}
	}
}
