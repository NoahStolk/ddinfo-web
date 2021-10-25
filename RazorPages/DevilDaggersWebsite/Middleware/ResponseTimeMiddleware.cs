using DevilDaggersWebsite.Singletons;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Middleware
{
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
			PathString path = context.Request.Path;
			string pathString = path.ToString();
			if (pathString.EndsWith(".png")
			 || pathString.EndsWith(".jpg")
			 || pathString.EndsWith(".css")
			 || pathString.EndsWith(".js")
			 || pathString.EndsWith(".ico")
			 || pathString.EndsWith(".gif")
			 || pathString.EndsWith(".ttf")
			 || pathString.EndsWith(".json"))
			{
				return _next(context);
			}

			Stopwatch sw = Stopwatch.StartNew();
			context.Response.OnStarting(() =>
			{
				sw.Stop();

				_responseTimeMonitor.Add(pathString, sw.ElapsedTicks, DateTime.UtcNow);

				return Task.CompletedTask;
			});

			return _next(context);
		}
	}
}
