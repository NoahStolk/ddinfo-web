using DevilDaggersInfo.Web.Server.Services;

namespace DevilDaggersInfo.Web.Server.Middleware;

public class ResponseTimeMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ResponseTimeMonitor _responseTimeMonitor;

	public ResponseTimeMiddleware(RequestDelegate next, ResponseTimeMonitor responseTimeMonitor)
	{
		_next = next;
		_responseTimeMonitor = responseTimeMonitor;
	}

	public Task InvokeAsync(HttpContext context)
	{
		if (!context.Request.Path.ToString().StartsWith("/api"))
			return _next(context);

		Stopwatch sw = Stopwatch.StartNew();
		context.Response.OnStarting(() =>
		{
			sw.Stop();

			// Longest time represented in ticks that fits in a 32-bit signed integer is around 3 minutes and 34 seconds.
			int trackedTicks = sw.ElapsedTicks > int.MaxValue ? int.MaxValue : (int)sw.ElapsedTicks;

			_responseTimeMonitor.Add(context.Request.Method, context.Request.Path.ToString(), trackedTicks, DateTime.UtcNow);

			return Task.CompletedTask;
		});

		return _next(context);
	}
}
