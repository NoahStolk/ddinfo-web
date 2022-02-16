using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;
using System.Globalization;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Utils;

public static class ReflectionUtils
{
	private static void UseUnderlyingNullableType(ref Type type)
	{
		Type? nullableType = Nullable.GetUnderlyingType(type);
		if (nullableType != null)
			type = nullableType;
	}

	public static string GetDtoPropertyDisplayValue<TDto>(PropertyInfo pi, TDto dto)
	{
		Type type = pi.PropertyType;
		object? value = pi.GetValue(dto);
		if (value == null)
			return string.Empty;

		UseUnderlyingNullableType(ref type);

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
