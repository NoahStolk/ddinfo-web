using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.HostedServices
{
	public class DevilDaggersDiscordBotService : IHostedService
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
