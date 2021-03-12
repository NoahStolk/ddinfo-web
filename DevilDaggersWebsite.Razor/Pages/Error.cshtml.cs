using DevilDaggersCore.Utils;
using DevilDaggersDiscordBot.Extensions;
using DevilDaggersDiscordBot.Logging;
using DSharpPlus.Entities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Globalization;
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
					Title = "INTERNAL SERVER ERROR",
					Color = DiscordColor.Red,
				};

				IExceptionHandlerPathFeature? exceptionFeature = HttpContext.Features?.Get<IExceptionHandlerPathFeature>();
				builder.AddFieldObject("Timestamp", DateTime.UtcNow.ToString(FormatUtils.DateTimeFullFormat, CultureInfo.InvariantCulture), true);
				builder.AddFieldObject("Route", exceptionFeature?.Path, true);
				builder.AddFieldObject("Request query string", HttpContext.Request?.QueryString, true);
				if (exceptionFeature != null)
					builder.AddError(exceptionFeature.Error);

				await DiscordLogger.Instance.TryLog(Channel.ErrorMonitoring, _env.EnvironmentName, null, builder.Build());
			}
			catch (Exception ex)
			{
				await DiscordLogger.Instance.TryLog(Channel.ErrorMonitoring, _env.EnvironmentName, $"Error report '{nameof(ErrorModel)}' failed! {ex.Message}");
			}
		}
	}
}
