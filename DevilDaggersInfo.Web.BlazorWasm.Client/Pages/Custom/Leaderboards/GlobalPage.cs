using DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;
using DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Constants;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Custom.Leaderboards;

public partial class GlobalPage
{
	private int _category = 1;

	[Inject]
	public PublicApiHttpClient Http { get; set; } = null!;

	[Inject]
	public NavigationManager NavigationManager { get; set; } = null!;

	public GetGlobalCustomLeaderboard? GetGlobalCustomLeaderboard { get; set; }

	[Parameter]
	[SupplyParameterFromQuery]
	public int Category { get => _category; set => _category = Math.Max(1, value); }

	public CustomLeaderboardCategory CategoryEnum => (CustomLeaderboardCategory)Category;

	protected async override Task OnParametersSetAsync()
	{
		await Fetch();
	}

	private async Task Fetch()
	{
		GetGlobalCustomLeaderboard = await Http.GetGlobalCustomLeaderboardForCategory(CategoryEnum);
	}

	private async Task ChangeCategory(ChangeEventArgs e)
	{
		if (!Enum.TryParse<CustomLeaderboardCategory>(e.Value?.ToString(), out CustomLeaderboardCategory category))
			return;

		Category = (int)category;
		NavigationManager.AddOrModifyQueryParameter(QueryParameters.Category, Category);

		await Fetch();
	}

	private static class QueryParameters
	{
		public static string Category { get; } = nameof(Category);
	}
}
