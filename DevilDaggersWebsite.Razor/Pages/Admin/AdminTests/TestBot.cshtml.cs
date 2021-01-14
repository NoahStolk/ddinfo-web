using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using BotLogger = DiscordBotDdInfo.DiscordLogger;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AdminTests
{
	public class TestBotModel : PageModel
	{
		public async Task OnGetAsync()
			=> await BotLogger.Instance.TryLog("Hello, this is a test message sent from an external environment.");
	}
}
