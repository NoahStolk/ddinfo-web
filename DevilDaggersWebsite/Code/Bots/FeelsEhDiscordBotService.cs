using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Code.Bots
{
	public class FeelsEhDiscordBotService : IHostedService
	{
		public Task StartAsync(CancellationToken cancellationToken)
		{
			FeelsEhDiscordBot.Program.Main(new string[] { });

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}