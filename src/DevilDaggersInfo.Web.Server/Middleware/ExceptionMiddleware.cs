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
			context.Response.ContentType = "application/problem+json; charset=utf-8";

			await context.Response.WriteAsJsonAsync(new ProblemDetails
			{
				Status = (int)ex.StatusCode,
				Title = DisplayException(ex),
			});
		}

		static string DisplayException(Exception ex)
		{
			if (ex.InnerException == null)
				return ex.Message;

			return ex.Message + Environment.NewLine + DisplayException(ex.InnerException);
		}
	}
}
