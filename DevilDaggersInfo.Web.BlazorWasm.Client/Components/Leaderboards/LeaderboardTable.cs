using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Components.Leaderboards;

public partial class LeaderboardTable<TGetLeaderboardDto, TGetEntryDto>
	where TGetLeaderboardDto : class, IGetLeaderboardDto<TGetEntryDto>
	where TGetEntryDto : class, IGetEntryDto
{
	private int? _expandedId;
	private LeaderboardSorting _sortBy;
	private bool _ascending;

	private Dictionary<LeaderboardSorting, bool> _sortings = new();

	[Parameter] public bool IsHistory { get; set; }
	[Parameter, EditorRequired] public TGetLeaderboardDto Leaderboard { get; set; } = default!;
	[Parameter, EditorRequired] public List<GetPlayerForLeaderboard> Players { get; set; } = null!;
	[Parameter, EditorRequired] public GameVersion GameVersion { get; set; }

	protected override void OnInitialized()
	{
		foreach (LeaderboardSorting e in (LeaderboardSorting[])Enum.GetValues(typeof(LeaderboardSorting)))
			_sortings.Add(e, false);
	}

	private void Expand(int playerId)
	{
		if (_expandedId == playerId)
			_expandedId = null;
		else
			_expandedId = playerId;
	}

	private void Sort(LeaderboardSorting sortBy)
	{
		_sortBy = sortBy;
		_sortings[sortBy] = !_sortings[sortBy];
		_ascending = _sortings[sortBy];

		Leaderboard.Entries = _sortBy switch
		{
			LeaderboardSorting.Accuracy => Leaderboard.Entries.OrderBy(e => e.DaggersFired == 0 ? 0 : e.DaggersHit / (float)e.DaggersFired, _ascending).ToList(),
			LeaderboardSorting.DeathType => Leaderboard.Entries.OrderBy(e => e.DeathType, _ascending).ToList(),
			LeaderboardSorting.Flag => Leaderboard.Entries.OrderBy(e => Players.Find(p => p.Id == e.Id)?.CountryCode ?? string.Empty, _ascending).ToList(),
			LeaderboardSorting.Gems => Leaderboard.Entries.OrderBy(e => e.Gems, _ascending).ToList(),
			LeaderboardSorting.Kills => Leaderboard.Entries.OrderBy(e => e.Kills, _ascending).ToList(),
			LeaderboardSorting.Player => Leaderboard.Entries.OrderBy(e => e.Username, _ascending).ToList(),
			LeaderboardSorting.Rank => Leaderboard.Entries.OrderBy(e => e.Rank, _ascending).ToList(),
			LeaderboardSorting.Time => Leaderboard.Entries.OrderBy(e => e.Time, _ascending).ToList(),
			LeaderboardSorting.TotalAccuracy => Leaderboard.Entries.OrderBy(e => e.DaggersFiredTotal == 0 ? 0 : e.DaggersHitTotal / (float)e.DaggersFiredTotal, _ascending).ToList(),
			LeaderboardSorting.TotalDeaths => Leaderboard.Entries.OrderBy(e => e.DeathsTotal, _ascending).ToList(),
			LeaderboardSorting.TotalGems => Leaderboard.Entries.OrderBy(e => e.GemsTotal, _ascending).ToList(),
			LeaderboardSorting.TotalKills => Leaderboard.Entries.OrderBy(e => e.KillsTotal, _ascending).ToList(),
			LeaderboardSorting.TotalTime => Leaderboard.Entries.OrderBy(e => e.TimeTotal, _ascending).ToList(),
			_ => Leaderboard.Entries,
		};
	}

	private enum LeaderboardSorting
	{
		Rank,
		Flag,
		Player,
		Time,
		Kills,
		Gems,
		Accuracy,
		DeathType,
		TotalTime,
		TotalKills,
		TotalGems,
		TotalAccuracy,
		TotalDeaths,
	}
}
