using DiscordBotDdInfo;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Bot = DiscordBotDdInfo.Program;

namespace DevilDaggersWebsite.Pages.Admin.AdminTests
{
	public class TestBotModel : PageModel
	{
		public async Task OnGetAsync()
		{
			await Bot.DevChannel.SendMessageAsyncSafe("Hello, this is a test message sent from an external environment.");
		}
	}
}