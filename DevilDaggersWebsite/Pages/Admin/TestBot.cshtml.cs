using CoreBase3.Services;
using DevilDaggersDiscordBot;
using DevilDaggersWebsite.Code.PageModels;
using System.Threading.Tasks;
using Bot = DevilDaggersDiscordBot.Program;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class TestBotModel : AdminPageModel
	{
		public TestBotModel(ICommonObjects commonObjects)
			: base(commonObjects)
		{
		}

		public async Task OnGetAsync()
		{
			await Bot.DevChannel.SendMessageAsyncSafe("Hello, this is a test message sent from an external environment.");
		}
	}
}