using DevilDaggersCore.Utils;
using DiscordBotDdInfo;
using DSharpPlus.Entities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
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

				IExceptionHandlerPathFeature exceptionFeature = HttpContext.Features?.Get<IExceptionHandlerPathFeature>();
				builder.AddFieldObject("Timestamp", DateTime.Now.ToString(FormatUtils.DateTimeFullFormat), true);
				builder.AddFieldObject("Route", exceptionFeature?.Path, true);
				builder.AddFieldObject("Request query string", HttpContext.Request?.QueryString, true);
				// builder.AddFieldObject("Request ID", Activity.Current?.Id ?? HttpContext.TraceIdentifier);
				// builder.AddFieldObject("Request method", HttpContext.Request?.Method);
				// builder.AddFieldObject("Content type", HttpContext.Request?.ContentType);
				// builder.AddFieldObject("Content length", HttpContext.Request?.ContentLength);
				builder.AddError(exceptionFeature?.Error);

				await Bot.DevChannel.SendMessageAsyncSafe(null, builder.Build());
			}
			catch (Exception ex)
			{
				await Bot.DevChannel.SendMessageAsyncSafe($"Error report failed! {ex.Message}");
			}
		}
	}
}