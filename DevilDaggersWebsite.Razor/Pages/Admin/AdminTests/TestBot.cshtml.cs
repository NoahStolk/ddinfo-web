using DiscordBotDdInfo.Logging;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AdminTests
{
	public class TestBotModel : PageModel
	{
		public async Task OnGetAsync()
			=> await DiscordLogger.Instance.TryLog(Channel.TestMonitoring, "Hello, this is a test message sent from an external environment.");
	}
}
