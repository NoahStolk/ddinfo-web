using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Bot = DevilDaggersDiscordBot.Program;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class TestBotModel : PageModel
	{
		public async Task<ActionResult> OnGetAsync()
		{
			await Bot.SendMessageAsyncSafe(Bot.DevChannel, "Hello, this is a test message sent from an external environment.");

			return RedirectToPage("/Admin/Index");
		}
	}
}