using System.Linq;
using System.Reflection;

namespace DevilDaggersWebsite.Utils
{
	public static class ReflectionUtils
	{
		public static PropertyInfo[] GetDtoDisplayPropertyInfos<TDto>()
			=> typeof(TDto).GetProperties().Where(pi => pi.CanWrite && (pi.PropertyType.IsValueType || pi.PropertyType == typeof(string))).ToArray();

		public static string GetDtoPropertyDisplayValue<TDto>(PropertyInfo pi, TDto entity)
			=> pi.GetValue(entity)?.ToString() ?? string.Empty;
	}
}
