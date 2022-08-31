using DevilDaggersInfo.Api.Main.LeaderboardHistory;
using DevilDaggersInfo.Api.Main.Players;
using DevilDaggersInfo.Types.Core.Wiki;
using DevilDaggersInfo.Web.Client.Extensions;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Pages.Leaderboard;

public partial class HistoryPage
{
	private const string _queryFormat = "yyyy-MM-dd-HH-mm";

	private bool _reloading;
	private DateTime _dateTime;
	private GameVersion _gameVersion;

	private int MaxRank => (GetLeaderboardHistory?.Entries.Count ?? int.MaxValue) - 99;

	[Parameter]
	[SupplyParameterFromQuery]
	public string? From { get; set; }

	[Parameter]
	public int Rank { get; set; } = 1;

	public GetLeaderboardHistory? GetLeaderboardHistory { get; set; }

	public List<GetPlayerForLeaderboard>? Players { get; set; }

	protected override async Task OnInitializedAsync()
	{
		Players = await Http.GetPlayersForLeaderboard();

		_dateTime = ParseQuery(From);

		await FetchLeaderboard();
	}

	private void SetRank(int value)
	{
		Rank = Math.Clamp(value, 1, MaxRank);
	}

	private async Task UpdateDateTime(DateTime dateTime)
	{
		_dateTime = dateTime;
		From = dateTime.ToString(_queryFormat);
		NavigationManager.AddOrModifyQueryParameter(QueryParameters.From, From);

		await FetchLeaderboard();
	}

	private async Task FetchLeaderboard()
	{
		if (GetLeaderboardHistory != null)
			_reloading = true;

		GetLeaderboardHistory = await Http.GetLeaderboardHistory(_dateTime);
		_reloading = false;
		_gameVersion = GameVersions.GetGameVersionFromDate(GetLeaderboardHistory.DateTime) ?? GameVersion.V1_0;
	}

	private static DateTime ParseQuery(string? query)
	{
		DateTime def = DateTime.UtcNow;
		if (string.IsNullOrWhiteSpace(query) || query.Length < 10)
			return def;

		if (!int.TryParse(query[..4], out int year))
			return def;

		if (!int.TryParse(query[5..7], out int month))
			return def;

		if (!int.TryParse(query[8..10], out int day))
			return def;

		if (query.Length < 13 || !int.TryParse(query[11..13], out int hour))
			hour = 0;

		if (query.Length < 16 || !int.TryParse(query[14..16], out int minute))
			minute = 0;

		return new(year, month, day, hour, minute, 0);
	}

	private static bool DatesEqual(DateTime a, DateTime b)
	{
		return a.Year == b.Year && a.Month == b.Month && a.Day == b.Day;
	}

	private static class QueryParameters
	{
		public static string From => nameof(From);
	}
}
