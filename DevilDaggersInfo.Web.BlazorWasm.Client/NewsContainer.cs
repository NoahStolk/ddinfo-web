using DevilDaggersInfo.Web.BlazorWasm.Client.InternalModels;

namespace DevilDaggersInfo.Web.BlazorWasm.Client;

/// <summary>
/// Include the news in the build so we don't rely on static files that will be caching incorrectly.
/// </summary>
public static class NewsContainer
{
	public static List<NewsItem> Items { get; } = new()
	{
		new()
		{
			DateTime = new(2022, 01, 02),
			Title = "V5 Beta",
		},
		new()
		{
			DateTime = new(2022, 01, 06),
			Title = "V5 RELEASED",
		},
		new()
		{
			DateTime = new(2022, 01, 21),
			Title = "V5.1 Update",
		},
		new()
		{
			DateTime = new(2022, 02, 06),
			Title = "V5.2 Update",
		},
		new()
		{
			DateTime = new(2022, 03, 02),
			Title = "V5.3 Update",
		},
		new()
		{
			DateTime = new(2022, 03, 18),
			Title = "V5.4 Update",
		},
	};
}
