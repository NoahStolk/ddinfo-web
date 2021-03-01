using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public class AdminEntityIndexPageModel<TEntity> : PageModel
	   where TEntity : class
	{
		private readonly ApplicationDbContext _dbContext;

		public AdminEntityIndexPageModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;

			DbSet = ((Array.Find(typeof(ApplicationDbContext).GetProperties(), pi => pi.PropertyType == typeof(DbSet<TEntity>)) ?? throw new("Could not retrieve DbSet of TEntity.")).GetValue(_dbContext) as DbSet<TEntity>)!;

			EntityProperties = typeof(TEntity).GetProperties().Where(pi => !pi.PropertyType.IsGenericType).ToArray();
		}

		public DbSet<TEntity> DbSet { get; } = null!;

		public PropertyInfo[] EntityProperties { get; } = null!;

		public string GetString(PropertyInfo pi, TEntity entity)
			=> pi.GetValue(entity)?.ToString() ?? string.Empty;
	}
}
