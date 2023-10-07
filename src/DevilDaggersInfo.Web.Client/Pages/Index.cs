using DevilDaggersInfo.Web.Client.HttpClients;
using DevilDaggersInfo.Web.Client.Models.Internal;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Pages;

public partial class Index
{
	private readonly Section[] _sections =
	{
		new("View the official leaderboard", "home/leaderboard.png", "/leaderboard", "Leaderboard"),
		new("Check out mods made by the community", "home/mod.png", "/custom/mods", "Mods"),
		new("Check out spawnsets made by the community", "home/spawnset.png", "/custom/spawnsets", "Spawnsets"),
		new("Check out the asset editor which can be used to mod the game", "tools/ddae.png", "/tools/asset-editor", "Asset Editor"),
		new("Check out the survival editor which can be used to create spawnsets and practice sections of the game", "tools/ddse.png", "/tools/survival-editor", "Spawnset Editor"),
		new("Participate in custom leaderboards for spawnsets", "home/custom-leaderboard.png", "/custom/leaderboards", "Custom Leaderboards"),
		new("Check out the world record progression and other statistics", "home/graph.png", "/leaderboard/world-record-progression", "World Record Progression"),
		new("Check out various wiki pages about the game, such as spawn times, enemy types, or hand upgrades", "home/enemies.png", "/wiki/enemies", "Enemies Wiki"),
	};

	[Inject]
	public required MainApiHttpClient Http { get; set; }

	public NewsItem? LatestNews { get; set; }

	public MarkupString? LatestNewsContent { get; set; }

	protected override async Task OnInitializedAsync()
	{
		LatestNews = NewsContainer.Items.MaxBy(ni => ni.DateTime);
		if (LatestNews == null)
			return;

		string html = await Http.Client.GetStringAsync(LatestNews.GetHtmlPath());
		LatestNewsContent = new(html);
	}

	private sealed record Section(string Text, string ImageUrl, string Url, string Alt);
}
