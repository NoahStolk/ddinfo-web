namespace DevilDaggersWebsite.BlazorWasm.Server.Extensions
{
	public static class StructExtensions
	{
		public static T? NullIfDefault<T>(this T value)
			where T : struct
			=> value.Equals(default) ? null : value;
	}
}
