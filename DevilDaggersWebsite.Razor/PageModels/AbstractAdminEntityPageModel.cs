using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
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

			EntityDisplayProperties = typeof(TEntity).GetProperties().Where(pi => pi.CanWrite && (pi.PropertyType.IsValueType || pi.PropertyType == typeof(string))).ToArray();
		}

		protected ApplicationDbContext DbContext { get; }

		public DbSet<TEntity> DbSet { get; }

		public PropertyInfo[] EntityDisplayProperties { get; }
	}
}
