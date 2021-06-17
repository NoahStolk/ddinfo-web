using DevilDaggersWebsite.Singletons;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Middleware
{
	public class ResponseTimeMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ResponseTimeMiddleware> _logger;
		private readonly ResponseTimeLogger _responseTimeLogger;

		public ResponseTimeMiddleware(RequestDelegate next, ILogger<ResponseTimeMiddleware> logger, ResponseTimeLogger responseTimeLogger)
		{
			_next = next;
			_logger = logger;
			_responseTimeLogger = responseTimeLogger;
		}

		public Task InvokeAsync(HttpContext context)
		{
			PathString path = context.Request.Path;
			string pathString = path.ToString();
			if (pathString.EndsWith(".png") || pathString.EndsWith(".jpg") || pathString.EndsWith(".css") || pathString.EndsWith(".js") || pathString.EndsWith(".ico") || pathString.EndsWith(".gif"))
				return _next(context);

			Stopwatch sw = Stopwatch.StartNew();
			context.Response.OnStarting(() =>
			{
				sw.Stop();

				long responseTimeMicroseconds = sw.ElapsedTicks / 10; // There are 10 ticks in a microsecond.
				_logger.LogDebug($"Response time (μs) for {context.Request.Path}: {responseTimeMicroseconds}");
				_responseTimeLogger.Log(new()
				{
					DateTime = DateTime.UtcNow,
					Path = pathString.Substring(0, Math.Min(64, pathString.Length)),
					ResponseTimeMicroseconds = responseTimeMicroseconds > int.MaxValue ? int.MaxValue : (int)responseTimeMicroseconds,
				});

				return Task.CompletedTask;
			});

			return _next(context);
		}
	}
}
