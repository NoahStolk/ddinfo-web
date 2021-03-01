using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public abstract class AbstractAdminEntityPageModel<TEntity> : PageModel
	   where TEntity : class, IEntity
	{
		protected AbstractAdminEntityPageModel(ApplicationDbContext dbContext)
		{
			DbContext = dbContext;

			DbSet = ((Array.Find(typeof(ApplicationDbContext).GetProperties(), pi => pi.PropertyType == typeof(DbSet<TEntity>)) ?? throw new("Could not retrieve DbSet of TEntity.")).GetValue(DbContext) as DbSet<TEntity>)!;

			EntityProperties = typeof(TEntity).GetProperties().Where(pi => !(pi.PropertyType.IsGenericType && typeof(IList).IsAssignableFrom(pi.PropertyType)) && pi.CanWrite).ToArray();
		}

		protected ApplicationDbContext DbContext { get; }

		public DbSet<TEntity> DbSet { get; }

		public PropertyInfo[] EntityProperties { get; }
	}
}
