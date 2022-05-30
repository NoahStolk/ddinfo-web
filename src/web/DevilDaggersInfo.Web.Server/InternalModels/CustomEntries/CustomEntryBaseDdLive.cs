using DevilDaggersInfo.Web.Server.Contracts;

namespace DevilDaggersInfo.Web.Server.InternalModels.CustomEntries;

public record CustomEntryBaseDdLive(
	int Time,
	DateTime SubmitDate,
	int CustomLeaderboardId,
	int PlayerId,
	string PlayerName) : ISortableCustomEntry;
