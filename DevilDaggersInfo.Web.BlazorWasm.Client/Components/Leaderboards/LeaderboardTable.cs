using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Components.Leaderboards;

public partial class LeaderboardTable<TGetEntryDto>
	where TGetEntryDto : class, IGetEntryDto
{
	private int? _expandedId;
	private LeaderboardSorting _sortBy;
	private bool _ascending;

	private Dictionary<LeaderboardSorting, bool> _sortings = new();

	[Parameter] public bool IsHistory { get; set; }
	[Parameter, EditorRequired] public List<TGetEntryDto> Entries { get; set; } = default!;
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

		Entries = _sortBy switch
		{
			LeaderboardSorting.Accuracy => Entries.OrderBy(e => e.DaggersFired == 0 ? 0 : e.DaggersHit / (float)e.DaggersFired, _ascending).ToList(),
			LeaderboardSorting.DeathType => Entries.OrderBy(e => e.DeathType, _ascending).ToList(),
			LeaderboardSorting.Flag => Entries.OrderBy(e => Players.Find(p => p.Id == e.Id)?.CountryCode ?? string.Empty, _ascending).ToList(),
			LeaderboardSorting.Gems => Entries.OrderBy(e => e.Gems, _ascending).ToList(),
			LeaderboardSorting.Kills => Entries.OrderBy(e => e.Kills, _ascending).ToList(),
			LeaderboardSorting.Player => Entries.OrderBy(e => e.Username, _ascending).ToList(),
			LeaderboardSorting.Rank => Entries.OrderBy(e => e.Rank, _ascending).ToList(),
			LeaderboardSorting.Time => Entries.OrderBy(e => e.Time, _ascending).ToList(),
			LeaderboardSorting.TotalAccuracy => Entries.OrderBy(e => e.DaggersFiredTotal == 0 ? 0 : e.DaggersHitTotal / (float)e.DaggersFiredTotal, _ascending).ToList(),
			LeaderboardSorting.TotalDeaths => Entries.OrderBy(e => e.DeathsTotal, _ascending).ToList(),
			LeaderboardSorting.TotalGems => Entries.OrderBy(e => e.GemsTotal, _ascending).ToList(),
			LeaderboardSorting.TotalKills => Entries.OrderBy(e => e.KillsTotal, _ascending).ToList(),
			LeaderboardSorting.TotalTime => Entries.OrderBy(e => e.TimeTotal, _ascending).ToList(),
			_ => Entries,
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
