using Microsoft.Extensions.Primitives;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;

public static class QueryExtensions
{
	private static T GetValueOrDefault<T>(this Dictionary<string, StringValues> query, string key, Func<string, T> converter)
		where T : struct
	{
		if (!query.TryGetValue(key, out StringValues valueAsStringValues))
			return default;

		return converter.Invoke(valueAsStringValues);
	}

	public static int GetIntOrDefault(this Dictionary<string, StringValues> query, string key)
		=> GetValueOrDefault(query, key, (s) => int.TryParse(s, out int result) ? result : default);

	public static bool GetBoolOrDefault(this Dictionary<string, StringValues> query, string key)
		=> GetValueOrDefault(query, key, (s) => bool.TryParse(s, out bool result) && result);

	public static TEnum GetEnumOrDefault<TEnum>(this Dictionary<string, StringValues> query, string key)
		where TEnum : struct, Enum
		=> GetValueOrDefault(query, key, (s) => Enum.TryParse(s, out TEnum result) ? result : default);

	public static string? GetStringOrDefault(this Dictionary<string, StringValues> query, string key)
		=> query.TryGetValue(key, out StringValues valueAsStringValues) ? valueAsStringValues.ToString() : null;
}
