using DevilDaggersDiscordBot;
using DevilDaggersDiscordBot.Extensions;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Singletons;
using DSharpPlus.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.HostedServices
{
	public class DatabaseLoggerBackgroundService : AbstractBackgroundService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public DatabaseLoggerBackgroundService(IWebHostEnvironment environment, BackgroundServiceMonitor backgroundServiceMonitor, IServiceScopeFactory serviceScopeFactory)
			: base(environment, backgroundServiceMonitor)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

		protected override TimeSpan Interval => TimeSpan.FromHours(1);

		protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
		{
			if (ServerConstants.DatabaseMessage == null)
				return;

			ApplicationDbContext dbContext = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();

			IQueryable<InformationSchemaTable> tables = dbContext.InformationSchemaTables.FromSqlRaw($@"SELECT
	table_name AS `{nameof(InformationSchemaTable.Table)}`,
	data_length `{nameof(InformationSchemaTable.DataSize)}`,
	index_length `{nameof(InformationSchemaTable.IndexSize)}`,
	avg_row_length `{nameof(InformationSchemaTable.AverageRowLength)}`,
	table_rows `{nameof(InformationSchemaTable.TableRows)}`
FROM information_schema.TABLES
WHERE table_schema = 'devildaggers'
ORDER BY table_name ASC;");

			DiscordEmbedBuilder builder = new()
			{
				Title = $"Database {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}",
				Color = DiscordColor.White,
			};
			foreach (InformationSchemaTable table in tables)
			{
				string value = @$"`{"DataSize",-10}{table.DataSize,7}`
`{"IxSize",-10}{table.IndexSize,7}`
`{"AvgRL",-10}{table.AverageRowLength,7}`
`{"Count",-10}{table.TableRows,7}`";
				builder.AddFieldObject(table.Table ?? "Null", value, true);
			}

			await DiscordLogger.EditMessage(ServerConstants.DatabaseMessage, builder.Build());
		}
	}
}
