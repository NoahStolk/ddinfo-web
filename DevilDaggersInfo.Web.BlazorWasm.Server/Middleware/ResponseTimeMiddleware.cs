namespace DevilDaggersInfo.Web.BlazorWasm.Server.Middleware;

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
		Stopwatch sw = Stopwatch.StartNew();
		context.Response.OnStarting(() =>
		{
			sw.Stop();

			_responseTimeMonitor.Add(context.Request.Path.ToString(), sw.ElapsedTicks, DateTime.UtcNow);

			return Task.CompletedTask;
		});

		return _next(context);
	}
}
