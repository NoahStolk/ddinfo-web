using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace DevilDaggersWebsite.BlazorWasm.Client.Utils
{
	public static class ReflectionUtils
	{
		public static PropertyInfo[] GetDtoDisplayPropertyInfos<TDto>()
			=> typeof(TDto).GetProperties().Where(pi => pi.CanWrite && (pi.PropertyType.IsValueType || pi.PropertyType == typeof(string))).ToArray();

		public static string GetDtoPropertyDisplayValue<TDto>(PropertyInfo pi, TDto dto)
		{
			Type type = pi.PropertyType;
			object? value = pi.GetValue(dto);
			if (value == null)
				return string.Empty;

			Type? nullableType = Nullable.GetUnderlyingType(type);
			if (nullableType != null)
				type = nullableType;

			if (type == typeof(DateTime))
				return ((DateTime)value).ToString(FormatUtils.DateTimeUtcFormat);

			if (type.GetInterfaces().Any(i => i == typeof(IFormattable)))
			{
				FormatAttribute? fa = pi.GetCustomAttribute<FormatAttribute>();
				if (fa != null)
					return ((IFormattable)value).ToString(fa.Format, CultureInfo.InvariantCulture);
			}

			return pi.GetValue(dto)?.ToString() ?? string.Empty;
		}
	}
}
