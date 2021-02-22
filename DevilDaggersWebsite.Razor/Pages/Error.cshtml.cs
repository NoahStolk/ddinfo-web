using DevilDaggersCore.Utils;
using DiscordBotDdInfo.Extensions;
using DiscordBotDdInfo.Logging;
using DSharpPlus.Entities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages
{
	public class ErrorModel : PageModel
	{
		public async Task OnGetAsync()
		{
			try
			{
				DiscordEmbedBuilder builder = new DiscordEmbedBuilder
				{
					Title = "INTERNAL SERVER ERROR",
					Color = DiscordColor.Red,
				};

				IExceptionHandlerPathFeature? exceptionFeature = HttpContext.Features?.Get<IExceptionHandlerPathFeature>();
				builder.AddFieldObject("Timestamp", DateTime.Now.ToString(FormatUtils.DateTimeFullFormat, CultureInfo.InvariantCulture), true);
				builder.AddFieldObject("Route", exceptionFeature?.Path, true);
				builder.AddFieldObject("Request query string", HttpContext.Request?.QueryString, true);
				if (exceptionFeature != null)
					builder.AddError(exceptionFeature.Error);

				await DiscordLogger.Instance.TryLog(LoggingChannel.Error, null, builder.Build());
			}
			catch (Exception ex)
			{
				await DiscordLogger.Instance.TryLog(LoggingChannel.Error, $"Error report '{nameof(ErrorModel)}' failed! {ex.Message}");
			}
		}
	}
}
