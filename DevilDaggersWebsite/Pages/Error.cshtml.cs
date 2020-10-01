using DevilDaggersCore.Utils;
using DiscordBotDdInfo.Extensions;
using DSharpPlus.Entities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Globalization;
using System.Threading.Tasks;
using BotLogger = DiscordBotDdInfo.DiscordLogger;

namespace DevilDaggersWebsite.Pages
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

				await BotLogger.Instance.TryLog(null, builder.Build());
			}
			catch (Exception ex)
			{
				await BotLogger.Instance.TryLog($"Error report '{nameof(ErrorModel)}' failed! {ex.Message}");
			}
		}
	}
}