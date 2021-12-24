using DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Leaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Leaderboard;

public partial class SearchPage
{
	private string? _apiError;
	private bool _reloading;

	[Parameter, SupplyParameterFromQuery] public string? Username { get; set; }

	public List<GetEntry>? GetEntries { get; set; }

	public List<GetPlayerForLeaderboard>? Players { get; set; }

	protected override async Task OnInitializedAsync()
	{
		Players = await Http.GetPlayersForLeaderboard();
		await FetchLeaderboard();
	}

	private async Task SetUsername(string? username)
	{
		Username = username;
		NavigationManager.AddOrModifyQueryParameter(QueryParameters.Username, Username);

		await FetchLeaderboard();
	}

	private async Task FetchLeaderboard()
	{
		if (Username == null || Username.Length < 3)
			return;

		try
		{
			if (GetEntries != null)
				_reloading = true;

			GetEntries = (await Http.GetEntriesByName(Username)).OrderBy(e => e.Rank, true).ToList();

			_reloading = false;
		}
		catch (Exception ex)
		{
			_apiError = ex.Message;
		}
	}

	private async Task ChangeInputUsername(ChangeEventArgs e)
	{
		await SetUsername(e.Value?.ToString());
	}

	private static class QueryParameters
	{
		public static string Username { get; } = nameof(Username);
	}
}
