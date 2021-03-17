using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DevilDaggersWebsite.Entities
{
	public interface IAdminUpdatableEntity<TAdminDto> : IEntity
		where TAdminDto : class
	{
		public void Create(ApplicationDbContext dbContext, TAdminDto adminDto, StringBuilder auditLogger);
		public void Edit(ApplicationDbContext dbContext, TAdminDto adminDto, StringBuilder auditLogger);
		public void CreateManyToManyRelations(ApplicationDbContext dbContext, TAdminDto adminDto, StringBuilder auditLogger);
		public TAdminDto Populate();

		public void TrackEditUpdates(StringBuilder auditLogger, TAdminDto adminDto, Type entityType)
		{
			List<(PropertyInfo? EntityProperty, PropertyInfo DtoProperty)> properties = new();

			foreach (PropertyInfo dtoProperty in typeof(TAdminDto).GetProperties())
			{
				PropertyInfo? entityProperty = entityType.GetProperty(dtoProperty.Name);
				properties.Add((entityProperty, dtoProperty));
			}

			foreach ((PropertyInfo? entityProperty, PropertyInfo dtoProperty) in properties)
			{
				object? entityValue = entityProperty?.GetValue(this);
				object? dtoValue = dtoProperty.GetValue(adminDto);

				string? dtoValueString = dtoValue?.ToString();
				if (dtoValue?.GetType().IsGenericType == true && dtoValue is System.Collections.IEnumerable enumerable)
				{
					StringBuilder sb = new();
					foreach (object item in enumerable)
						sb.Append(item).Append(", ");

					dtoValueString = sb.ToString().TrimEnd(',', ' ');
				}

				if (entityValue?.ToString() != dtoValueString)
					auditLogger.AppendFormat("{0,20}", dtoProperty.Name).Append(": ").AppendFormat("{0,20}", entityValue).Append(" -> ").AppendFormat("{0,20}", dtoValueString).AppendLine();
			}
		}
	}
}
