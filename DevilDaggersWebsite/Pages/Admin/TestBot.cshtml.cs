using DevilDaggersDiscordBot;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Bot = DevilDaggersDiscordBot.Program;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class TestBotModel : AdminPageModel
	{
		public TestBotModel(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
			: base(httpContextAccessor, env)
		{
		}

		public async Task OnGetAsync()
		{
			await Bot.DevChannel.SendMessageAsyncSafe("Hello, this is a test message sent from an external environment.");
		}
	}
}