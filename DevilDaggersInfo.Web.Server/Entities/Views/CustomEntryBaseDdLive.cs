namespace DevilDaggersInfo.Web.Server.Entities.Views;

public record CustomEntryBaseDdLive(
	int Time,
	DateTime SubmitDate,
	int CustomLeaderboardId,
	int PlayerId,
	string PlayerName) : ISortableCustomEntry;
