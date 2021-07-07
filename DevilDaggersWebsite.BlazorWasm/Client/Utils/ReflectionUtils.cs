using System.Linq;
using System.Reflection;

namespace DevilDaggersWebsite.BlazorWasm.Client.Utils
{
	public static class ReflectionUtils
	{
		public static PropertyInfo[] GetDtoDisplayPropertyInfos<TDto>()
			=> typeof(TDto).GetProperties().Where(pi => pi.CanWrite && (pi.PropertyType.IsValueType || pi.PropertyType == typeof(string))).ToArray();

		public static string GetDtoPropertyDisplayValue<TDto>(PropertyInfo pi, TDto dto)
			=> pi.GetValue(dto)?.ToString() ?? string.Empty;
	}
}
