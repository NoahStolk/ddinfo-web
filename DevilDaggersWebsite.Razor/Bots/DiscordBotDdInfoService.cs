using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Bots
{
	public class DiscordBotDdInfoService : IHostedService
	{
		public Task StartAsync(CancellationToken cancellationToken)
		{
			DiscordBotDdInfo.Program.Main();

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}
