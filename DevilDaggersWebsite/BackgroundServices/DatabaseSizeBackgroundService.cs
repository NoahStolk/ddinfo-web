using DevilDaggersDiscordBot;
using DevilDaggersWebsite.Entities;
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
	public class DatabaseSizeBackgroundService : AbstractBackgroundService
	{
		private readonly IWebHostEnvironment _environment;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public DatabaseSizeBackgroundService(IWebHostEnvironment environment, IServiceScopeFactory serviceScopeFactory)
			: base(environment)
		{
			_environment = environment;
			_serviceScopeFactory = serviceScopeFactory;
		}

		protected override TimeSpan Interval => TimeSpan.FromSeconds(5); // TODO: 1 hour

		protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
		{
			ApplicationDbContext dbContext = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();

			IQueryable<InformationSchemaTable> tables = dbContext.InformationSchemaTables.FromSqlRaw(@"SELECT
	table_name AS `Table`,
	data_length `DataSize`,
	index_length `IndexSize`,
	avg_row_length `AverageRowLength`,
	table_rows `TableRows`
FROM information_schema.TABLES
WHERE table_schema = 'devildaggers'
ORDER BY table_name ASC;");

			StringBuilder sb = new("```");
			sb.AppendFormat("{0,-21}", "Table")
				.AppendFormat("{0,12}", "Data size")
				.AppendFormat("{0,12}", "Index size")
				.AppendFormat("{0,20}", "Average row length")
				.AppendFormat("{0,12}", "Table rows")
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

			// TODO: Edit Discord message.
			await DiscordLogger.TryLog(Channel.MonitoringTest, _environment.EnvironmentName, sb.ToString());
		}
	}
}
