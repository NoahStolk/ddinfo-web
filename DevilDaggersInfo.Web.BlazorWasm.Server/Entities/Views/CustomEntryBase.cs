namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities.Views;

public record CustomEntryBase(
	int Time,
	DateTime SubmitDate,
	int CustomLeaderboardId,
	string PlayerName) : ISortableCustomEntry;
