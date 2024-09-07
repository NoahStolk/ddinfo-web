using DevilDaggersInfo.Core.Common;

namespace DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardStatistics;

public class ArrayStatistics
{
	public ArrayStatistic Times { get; } = new();
	public ArrayStatistic Kills { get; } = new();
	public ArrayStatistic Gems { get; } = new();
	public ArrayStatistic DaggersFired { get; } = new();
	public ArrayStatistic DaggersHit { get; } = new();
	public ArrayStatistic Accuracy { get; } = new();

	public void Populate(List<CompressedEntry> entries, int? limit = null)
	{
		if (limit.HasValue)
			entries = entries.Take(limit.Value).ToList();

		Times.Populate(entries.ConvertAll(e => GameTime.FromGameUnits(e.Time).Seconds), d => d);
		Kills.Populate(entries.ConvertAll(e => (double)e.Kills), d => d);
		Gems.Populate(entries.ConvertAll(e => (double)e.Gems), d => d);
		DaggersFired.Populate(entries.ConvertAll(e => (double)e.DaggersFired), d => Math.Round(d / 100) * 100);
		DaggersHit.Populate(entries.ConvertAll(e => (double)e.DaggersHit), d => Math.Round(d / 100) * 100);
		Accuracy.Populate(entries.ConvertAll(e => e.DaggersFired == 0 ? 0 : e.DaggersHit / (double)e.DaggersFired * 100), Math.Floor);
	}
}
