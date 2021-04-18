using DevilDaggersWebsite.Entities;
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

		public string GetPropertyValueString(PropertyInfo pi, TEntity entity)
			=> pi.GetValue(entity)?.ToString() ?? string.Empty;
	}
}
