using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Middleware;

public class ExceptionMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionMiddleware> _logger;

	public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (StatusCodeException ex)
		{
			if (context.Response.HasStarted)
			{
				_logger.LogWarning(ex, "The response has already started, the exception middleware will not be executed.");
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
