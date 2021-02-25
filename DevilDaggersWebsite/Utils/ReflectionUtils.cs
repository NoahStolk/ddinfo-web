using System;

namespace DevilDaggersWebsite.Utils
{
	public static class ReflectionUtils
	{
		public static object? GetDefaultValue(Type type)
		{
			if (type.IsValueType)
				return Activator.CreateInstance(type);

			if (type == typeof(string))
				return string.Empty;

			return null;
		}

		public static object? GetDefaultValue<T>()
			=> GetDefaultValue(typeof(T));
	}
}
