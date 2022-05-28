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
				_logger.LogWarning("The response has already started, the exception middleware will not be executed.");
				throw;
			}

			context.Response.Clear();
			context.Response.StatusCode = 404;
			context.Response.ContentType = "application/problem+json; charset=utf-8";

			await context.Response.WriteAsJsonAsync(new ProblemDetails
			{
				Status = (int)ex.StatusCode,
				Title = ex.StatusCode.ToString(),
				Detail = DisplayException(ex),
			});
		}

		static string DisplayException(Exception ex)
		{
			string message = $"{ex.GetType().Name}: {ex.Message}";

			if (ex.InnerException == null)
				return message;

			return message + Environment.NewLine + DisplayException(ex.InnerException);
		}
	}
}
