using DevilDaggersCore.Utils;
using DevilDaggersDiscordBot.Extensions;
using DevilDaggersDiscordBot.Logging;
using DSharpPlus.Entities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages
{
	public class ErrorModel : PageModel
	{
		private readonly IWebHostEnvironment _env;

		public ErrorModel(IWebHostEnvironment env)
		{
			_env = env;
		}

		public async Task OnGetAsync()
		{
			try
			{
				DiscordEmbedBuilder builder = new()
				{
					Title = "Internal Server Error",
					Color = DiscordColor.Red,
				};

				IExceptionHandlerPathFeature? exceptionFeature = HttpContext?.Features?.Get<IExceptionHandlerPathFeature>();
				builder.AddFieldObject("Environment", _env.EnvironmentName, true);
				builder.AddFieldObject("Timestamp", DateTime.UtcNow.ToString(FormatUtils.DateTimeFullFormat), true);
				builder.AddFieldObject("Request query string", HttpContext?.Request?.QueryString, true);

				if (exceptionFeature != null)
				{
					builder.AddFieldObject("Route", exceptionFeature.Path, true);
					if (exceptionFeature.Error.StackTrace != null)
					{
						string stackTrace = exceptionFeature.Error.StackTrace;
						builder.AddFieldObject("Stack trace", stackTrace.Substring(0, Math.Min(100, stackTrace.Length)));
					}

					builder.AddError(exceptionFeature.Error);
				}

				await DiscordLogger.Instance.TryLog(Channel.ErrorMonitoring, _env.EnvironmentName, null, builder.Build());
			}
			catch (Exception ex)
			{
				await DiscordLogger.Instance.TryLog(Channel.ErrorMonitoring, _env.EnvironmentName, $"Error report on {nameof(ErrorModel)} failed: {ex.Message}");
			}
		}
	}
}
