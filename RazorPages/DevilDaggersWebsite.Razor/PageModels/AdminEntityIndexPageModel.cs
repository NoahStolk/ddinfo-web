using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Extensions;
using DevilDaggersWebsite.Singletons;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Reflection;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public class AdminEntityIndexPageModel<TEntity> : AbstractAdminEntityPageModel<TEntity>
		where TEntity : class, IEntity
	{
		public AdminEntityIndexPageModel(ApplicationDbContext dbContext, IWebHostEnvironment environment, DiscordLogger discordLogger)
			: base(dbContext, environment, discordLogger)
		{
		}

		[FromQuery]
		public string? SortOrder { get; set; }

		public string GetPropertyValueString(PropertyInfo pi, TEntity entity)
			=> pi.GetValue(entity)?.ToString() ?? string.Empty;

		public IQueryable<TEntity> GetOrderedQuery()
			=> SortOrder != null ? DbSet.OrderByMember(SortOrder, true) : DbSet;
	}
}
