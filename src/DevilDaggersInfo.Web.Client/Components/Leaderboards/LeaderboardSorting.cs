namespace DevilDaggersInfo.Web.Client.Components.Leaderboards;

// This must not be nested inside LeaderboardTable<TGetEntryDto>. An enum nested in a generic type becomes a separate
// runtime type per instantiation, and Enum.GetValues<T> throws InvalidCastException for those on the WebAssembly runtime.
internal enum LeaderboardSorting
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
