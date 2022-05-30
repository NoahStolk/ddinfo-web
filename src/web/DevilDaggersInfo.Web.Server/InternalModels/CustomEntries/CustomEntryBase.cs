using DevilDaggersInfo.Web.Server.Contracts;

namespace DevilDaggersInfo.Web.Server.InternalModels.CustomEntries;

public record CustomEntryBase(
	int Time,
	DateTime SubmitDate,
	int CustomLeaderboardId,
	string PlayerName) : ISortableCustomEntry;
