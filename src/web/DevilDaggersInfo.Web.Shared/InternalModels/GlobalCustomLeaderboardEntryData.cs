namespace DevilDaggersInfo.Web.Shared.InternalModels;

public record GlobalCustomLeaderboardEntryData
{
	public List<CustomLeaderboardRanking> Rankings { get; } = new();

	public int LeviathanCount { get; set; }

	public int DevilCount { get; set; }

	public int GoldenCount { get; set; }

	public int SilverCount { get; set; }

	public int BronzeCount { get; set; }

	public int DefaultCount { get; set; }

	public int TotalPlayed => LeviathanCount + DevilCount + GoldenCount + SilverCount + BronzeCount + DefaultCount;
}
