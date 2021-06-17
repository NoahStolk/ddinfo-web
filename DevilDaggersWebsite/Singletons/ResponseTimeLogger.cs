using DevilDaggersWebsite.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace DevilDaggersWebsite.Singletons
{
	public class ResponseTimeLogger
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public ResponseTimeLogger(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

		public void Log(ResponseLog responseLog)
		{
			IServiceScope scope = _serviceScopeFactory.CreateScope();
			ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			dbContext.ResponseLogs.Add(responseLog);
			dbContext.SaveChanges();
		}
	}
}
