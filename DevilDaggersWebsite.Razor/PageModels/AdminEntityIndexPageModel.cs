using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Reflection;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public class AdminEntityIndexPageModel<TEntity> : AbstractAdminEntityPageModel<TEntity>
		where TEntity : class, IEntity
	{
		public AdminEntityIndexPageModel(ApplicationDbContext dbContext)
			: base(dbContext)
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
