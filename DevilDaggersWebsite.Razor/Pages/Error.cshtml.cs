using DevilDaggersCore.Extensions;
using DevilDaggersWebsite.Extensions;
using DevilDaggersWebsite.HostedServices.DdInfoDiscordBot;
using DevilDaggersWebsite.Singletons;
using DevilDaggersWebsite.Utils;
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
		private readonly IWebHostEnvironment _environment;
		private readonly DiscordLogger _discordLogger;

		public ErrorModel(IWebHostEnvironment environment, DiscordLogger discordLogger)
		{
			_environment = environment;
			_discordLogger = discordLogger;
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
				builder.AddFieldObject("Environment", _environment.EnvironmentName, true);
				builder.AddFieldObject("Timestamp", DateTime.UtcNow.ToString(FormatUtils.DateTimeFullFormat), true);
				builder.AddFieldObject("Request query string", HttpContext?.Request?.QueryString, true);

				if (exceptionFeature != null)
				{
					builder.AddFieldObject("Route", exceptionFeature.Path, true);
					if (exceptionFeature.Error.StackTrace != null)
					{
						string stackTrace = exceptionFeature.Error.StackTrace;
						builder.AddFieldObject("Stack trace", stackTrace.TrimAfter(100));
					}

					builder.AddError(exceptionFeature.Error);
				}
				else
				{
					builder.AddError(new($"{nameof(IExceptionHandlerPathFeature)} is not available in the current HTTP context."));
				}

				await _discordLogger.TryLog(Channel.MonitoringError, null, builder.Build());
			}
			catch (Exception ex)
			{
				await _discordLogger.TryLog(Channel.MonitoringError, $"Error report on {nameof(ErrorModel)} failed: {ex.Message}");
			}
		}
	}
}
