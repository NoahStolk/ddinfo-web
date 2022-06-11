namespace DevilDaggersInfo.Web.Server.Domain.Models;

public record Page<T>
{
	public Page(List<T> results, int totalResults)
	{
		Results = results;
		TotalResults = totalResults;
	}

	public List<T> Results { get; }

	public int TotalResults { get; }
}
