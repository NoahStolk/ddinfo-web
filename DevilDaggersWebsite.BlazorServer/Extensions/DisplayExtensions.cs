namespace DevilDaggersWebsite.BlazorServer.Extensions
{
	public static class DisplayExtensions
	{
		public static string Pluralize(this int value)
			=> value == 1 ? string.Empty : "s";
	}
}
