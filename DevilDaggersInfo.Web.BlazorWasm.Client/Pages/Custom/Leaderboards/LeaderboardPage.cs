using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Custom.Leaderboards;

public partial class LeaderboardPage
{
	[Parameter, EditorRequired] public int Id { get; set; }

	public GetCustomLeaderboard? GetCustomLeaderboard { get; set; }

	private bool _notFound;
	private int? _expandedId;
	private CustomEntrySorting _sortBy;
	private bool _ascending;

	private Dictionary<CustomEntrySorting, bool> _sortings = new();

	protected override async Task OnInitializedAsync()
	{
		foreach (CustomEntrySorting e in (CustomEntrySorting[])Enum.GetValues(typeof(CustomEntrySorting)))
			_sortings.Add(e, false);

		try
		{
			GetCustomLeaderboard = await Http.GetCustomLeaderboardById(Id);
		}
		catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
		{
			_notFound = true;
		}
	}

	private void Expand(int playerId)
	{
		if (_expandedId == playerId)
			_expandedId = null;
		else
			_expandedId = playerId;
	}

	private void Sort(CustomEntrySorting sortBy)
	{
		_sortBy = sortBy;
		_sortings[sortBy] = !_sortings[sortBy];
		_ascending = _sortings[sortBy];

		GetCustomLeaderboard!.CustomEntries = _sortBy switch
		{
			CustomEntrySorting.Rank => GetCustomLeaderboard.CustomEntries.OrderBy(ce => ce.Rank, _ascending).ToList(),
			CustomEntrySorting.Flag => GetCustomLeaderboard.CustomEntries.OrderBy(ce => ce.CountryCode, _ascending).ToList(),
			CustomEntrySorting.Player => GetCustomLeaderboard.CustomEntries.OrderBy(ce => ce.PlayerName, _ascending).ToList(),
			CustomEntrySorting.Time => GetCustomLeaderboard.CustomEntries.OrderBy(ce => ce.Time, _ascending).ToList(),
			CustomEntrySorting.EnemiesKilled => GetCustomLeaderboard.CustomEntries.OrderBy(ce => ce.EnemiesKilled, _ascending).ToList(),
			CustomEntrySorting.EnemiesAlive => GetCustomLeaderboard.CustomEntries.OrderBy(ce => ce.EnemiesAlive, _ascending).ToList(),
			CustomEntrySorting.GemsCollected => GetCustomLeaderboard.CustomEntries.OrderBy(ce => ce.GemsCollected, _ascending).ToList(),
			CustomEntrySorting.GemsDespawned => GetCustomLeaderboard.CustomEntries.OrderBy(ce => ce.GemsDespawned, _ascending).ToList(),
			CustomEntrySorting.GemsEaten => GetCustomLeaderboard.CustomEntries.OrderBy(ce => ce.GemsEaten, _ascending).ToList(),
			CustomEntrySorting.Accuracy => GetCustomLeaderboard.CustomEntries.OrderBy(ce => ce.DaggersFired == 0 ? 0 : ce.DaggersHit / (float)ce.DaggersFired, _ascending).ToList(),
			CustomEntrySorting.DeathType => GetCustomLeaderboard.CustomEntries.OrderBy(ce => ce.DeathType, _ascending).ToList(),
			CustomEntrySorting.HomingStored => GetCustomLeaderboard.CustomEntries.OrderBy(ce => ce.HomingStored, _ascending).ToList(),
			CustomEntrySorting.HomingEaten => GetCustomLeaderboard.CustomEntries.OrderBy(ce => ce.HomingEaten, _ascending).ToList(),
			CustomEntrySorting.LevelUpTime2 => GetCustomLeaderboard.CustomEntries.OrderBy(ce => ce.LevelUpTime2, _ascending).ToList(),
			CustomEntrySorting.LevelUpTime3 => GetCustomLeaderboard.CustomEntries.OrderBy(ce => ce.LevelUpTime3, _ascending).ToList(),
			CustomEntrySorting.LevelUpTime4 => GetCustomLeaderboard.CustomEntries.OrderBy(ce => ce.LevelUpTime4, _ascending).ToList(),
			CustomEntrySorting.SubmitDate => GetCustomLeaderboard.CustomEntries.OrderBy(ce => ce.SubmitDate, _ascending).ToList(),
			_ => GetCustomLeaderboard.CustomEntries,
		};
	}

	private enum CustomEntrySorting
	{
		Rank,
		Flag,
		Player,
		Time,
		EnemiesKilled,
		EnemiesAlive,
		GemsCollected,
		GemsDespawned,
		GemsEaten,
		Accuracy,
		DeathType,
		HomingStored,
		HomingEaten,
		LevelUpTime2,
		LevelUpTime3,
		LevelUpTime4,
		SubmitDate,
	}
}
