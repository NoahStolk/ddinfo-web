using DevilDaggersDiscordBot;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Singletons;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BackgroundServices
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

			StringBuilder sb = new("```");
			sb.AppendFormat("{0,-21}", nameof(InformationSchemaTable.Table))
				.AppendFormat("{0,12}", nameof(InformationSchemaTable.DataSize))
				.AppendFormat("{0,12}", nameof(InformationSchemaTable.IndexSize))
				.AppendFormat("{0,20}", nameof(InformationSchemaTable.AverageRowLength))
				.AppendFormat("{0,12}", nameof(InformationSchemaTable.TableRows))
				.AppendLine();
			foreach (InformationSchemaTable ist in tables)
			{
				sb.AppendFormat("{0,-21}", ist.Table)
					.AppendFormat("{0,12}", ist.DataSize)
					.AppendFormat("{0,12}", ist.IndexSize)
					.AppendFormat("{0,20}", ist.AverageRowLength)
					.AppendFormat("{0,12}", ist.TableRows)
					.AppendLine();
			}

			sb.AppendLine("```");

			await DiscordLogger.EditMessage(ServerConstants.DatabaseMessage, sb.ToString());
		}
	}
}
