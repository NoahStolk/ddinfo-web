using DevilDaggersDiscordBot;
using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
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

		protected override TimeSpan Interval => default;// TimeSpan.FromHours(1);

		protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
		{
			try
			{
				ApplicationDbContext dbContext = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();

				IQueryable<InformationSchemaTable> tables = dbContext.InformationSchemaTables.FromSqlRaw("SELECT * FROM INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'devildaggers';");
				foreach (var ist in tables)
				{

				}
			}
			catch (Exception ex)
			{
				await DiscordLogger.TryLogException(ex.Message, _environment.EnvironmentName, ex);
			}

			//dbContext.Database.Select(db => new { db.index_length});
			//			dbContext.Database.ExecuteSqlRaw(@"SELECT 
			//    table_schema AS `Database`, 
			//    table_name AS `Table`, 
			//    data_length `Data size`,
			//    index_length `Index size`,
			//    data_length + index_length `Total size`
			//FROM information_schema.TABLES 
			//WHERE table_schema = 'devildaggers'
			//ORDER BY (data_length + index_length) DESC;");
		}
	}
}
