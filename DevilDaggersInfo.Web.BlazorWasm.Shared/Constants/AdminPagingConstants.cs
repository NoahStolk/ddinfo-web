namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Constants;

public static class AdminPagingConstants
{
	public const int PageSizeDefault = 30;
	public const int PageSizeMin = 10;
	public const int PageSizeMax = 50;

	public static IEnumerable<int> PageSizeOptions { get; } = Enumerable.Range(1, 5).Select(i => i * 10);
}
