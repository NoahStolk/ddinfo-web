namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Constants;

public static class PagingConstants
{
	public const int PageSizeDefault = 25;
	public const int PageSizeMin = 15;
	public const int PageSizeMax = 35;

	public static IEnumerable<int> PageSizeOptions { get; } = Enumerable.Range(3, 5).Select(i => i * 5);
}
