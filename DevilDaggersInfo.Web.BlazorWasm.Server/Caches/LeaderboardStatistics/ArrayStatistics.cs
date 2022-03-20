namespace DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardStatistics;

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

		Times.Populate(entries.Select(e => e.Time.ToSecondsTime()), d => d);
		Kills.Populate(entries.Select(e => (double)e.Kills), d => d);
		Gems.Populate(entries.Select(e => (double)e.Gems), d => d);
		DaggersFired.Populate(entries.Select(e => (double)e.DaggersFired), d => Math.Round(d / 100) * 100);
		DaggersHit.Populate(entries.Select(e => (double)e.DaggersHit), d => Math.Round(d / 100) * 100);
		Accuracy.Populate(entries.Select(e => e.DaggersFired == 0 ? 0 : e.DaggersHit / (double)e.DaggersFired * 100), d => Math.Floor(d));
	}
}
