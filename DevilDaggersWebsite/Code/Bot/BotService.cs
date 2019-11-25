using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Code.Bot
{
	public class BotService : IHostedService
	{
		public Task StartAsync(CancellationToken cancellationToken)
		{
			DevilDaggersDiscordBot.Program.Main();

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}