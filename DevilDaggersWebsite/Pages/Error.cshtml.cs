using DevilDaggersCore.Utils;
using DiscordBotDdInfo.Extensions;
using DSharpPlus.Entities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Bot = DiscordBotDdInfo.Program;

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

				if (Bot.DdInfoDevChannel != null)
					await Bot.DdInfoDevChannel.SendMessageAsyncSafe(null, builder.Build());
			}
			catch (Exception ex)
			{
				if (Bot.DdInfoDevChannel != null)
					await Bot.DdInfoDevChannel.SendMessageAsyncSafe($"Error report failed! {ex.Message}");
			}
		}
	}
}