using DevilDaggersInfo.Api.Main.Leaderboards;
using DevilDaggersInfo.Api.Main.Players;
using DevilDaggersInfo.Web.Client.Extensions;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Pages.Leaderboard;

public partial class Index
{
	private int _rank = 1;
	private string? _apiError;
	private bool _reloading;

	private int MaxRank => (GetLeaderboard?.TotalPlayers ?? int.MaxValue) - 99;

	[Parameter]
	[SupplyParameterFromQuery]
	public int Rank { get => _rank; set => _rank = Math.Max(1, value); }

	public GetLeaderboard? GetLeaderboard { get; set; }

	public List<GetPlayerForLeaderboard>? Players { get; set; }

	protected override async Task OnInitializedAsync()
	{
		Players = await Http.GetPlayersForLeaderboard();
		await FetchLeaderboard();
	}

	private async Task SetRank(int value)
	{
		Rank = Math.Clamp(value, 1, MaxRank);
		NavigationManager.AddOrModifyQueryParameter(QueryParameters.Rank, Rank);

		await FetchLeaderboard();
	}

	private async Task FetchLeaderboard()
	{
		try
		{
			if (GetLeaderboard != null)
				_reloading = true;

			GetLeaderboard = await Http.GetLeaderboard(Rank);
			_reloading = false;
		}
		catch (Exception ex)
		{
			_apiError = ex.Message;
		}
	}

	private async Task ChangeInputRank(ChangeEventArgs e)
	{
		if (int.TryParse(e.Value?.ToString(), out int result))
			await SetRank(result);
	}

	private static class QueryParameters
	{
		public static string Rank => nameof(Rank);
	}
}
