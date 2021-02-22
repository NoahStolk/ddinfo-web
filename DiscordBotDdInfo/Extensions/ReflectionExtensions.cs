using DiscordBotDdInfo.Attributes;
using System.Reflection;

namespace DiscordBotDdInfo.Extensions
{
	public static class ReflectionExtensions
	{
		public static bool IsSearchCommand(this MethodInfo methodInfo)
			=> methodInfo.GetCustomAttribute(typeof(SearchCommandAttribute)) != null;
	}
}
