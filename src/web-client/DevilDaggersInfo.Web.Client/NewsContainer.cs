using DevilDaggersInfo.Web.Client.InternalModels;

namespace DevilDaggersInfo.Web.Client;

/// <summary>
/// Include the news in the build so we don't rely on static files that might be cached incorrectly.
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
		new()
		{
			DateTime = new(2022, 04, 18),
			Title = "V5.5 Update",
		},
		new()
		{
			DateTime = new(2022, 07, 22),
			Title = "V5.6 Update and Replay Editor Alpha",
		},
	};
}
