namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities.Views;

public record CustomEntryBaseDdLive(
	int Time,
	DateTime SubmitDate,
	int CustomLeaderboardId,
	int PlayerId,
	string PlayerName) : ISortableCustomEntry;
