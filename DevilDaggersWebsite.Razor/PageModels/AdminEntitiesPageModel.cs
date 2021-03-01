using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public class AdminEntitiesPageModel<TEntity> : PageModel
	   where TEntity : class
	{
		private readonly ApplicationDbContext _dbContext;

		public AdminEntitiesPageModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public DbSet<TEntity> DbSet { get; private set; } = null!;

		public PropertyInfo[] EntityProperties { get; private set; } = null!;

		public void OnGet()
		{
			DbSet = ((Array.Find(typeof(ApplicationDbContext).GetProperties(), pi => pi.PropertyType == typeof(DbSet<TEntity>)) ?? throw new("Could not retrieve DbSet of TEntity.")).GetValue(_dbContext) as DbSet<TEntity>)!;

			EntityProperties = typeof(TEntity).GetProperties().Where(pi => !pi.PropertyType.IsGenericType).ToArray();
		}

		public string GetString(PropertyInfo pi, TEntity entity)
		{
			return pi.GetValue(entity)?.ToString() ?? string.Empty;
		}
	}
}
