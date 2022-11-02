using DevilDaggersInfo.Web.Client.HttpClients;
using DevilDaggersInfo.Web.Client.Models.Internal;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Pages;

public partial class Index
{
	private readonly Section[] _sections =
	{
		new("View the official leaderboard", "home/leaderboard.png", "/leaderboard"),
		new("Check out mods made by the community", "home/mod.png", "/custom/mods"),
		new("Check out spawnsets made by the community", "home/spawnset.png", "/custom/spawnsets"),
		new("Check out the asset editor which can be used to mod the game", "tools/ddae.png", "/tools/asset-editor"),
		new("Check out the survival editor which can be used to create spawnsets and practice sections of the game", "tools/ddse.png", "/tools/survival-editor"),
		new("Participate in custom leaderboards for spawnsets", "home/custom-leaderboard.png", "/custom/leaderboards"),
		new("Check out the world record progression and other statistics", "home/graph.png", "/leaderboard/world-record-progression"),
		new("Check out various wiki pages about the game, such as spawn times, enemy types, or hand upgrades", "home/enemies.png", "/wiki/enemies"),
	};

	[Inject]
	public MainApiHttpClient Http { get; set; } = null!;

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

	private record Section(string Text, string ImageUrl, string Url);
}
