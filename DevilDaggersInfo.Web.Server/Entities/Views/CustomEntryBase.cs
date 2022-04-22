namespace DevilDaggersInfo.Web.Server.Entities.Views;

public record CustomEntryBase(
	int Time,
	DateTime SubmitDate,
	int CustomLeaderboardId,
	string PlayerName) : ISortableCustomEntry;
