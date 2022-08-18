namespace DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardStatistics;

public class ArrayStatistic
{
	public double Average { get; private set; }
	public double Median { get; private set; }
	public double Mode { get; private set; }

	public void Populate(IEnumerable<double> data, Func<double, double> modeTransformer)
	{
		data = data.OrderBy(n => n);

		Average = data.Average();
		Median = data.ElementAt(data.Count() / 2);
		Mode = data
			.GroupBy(modeTransformer)
			.OrderByDescending(g => g.Count())
			.Select(g => g.Key)
			.FirstOrDefault();
	}
}
