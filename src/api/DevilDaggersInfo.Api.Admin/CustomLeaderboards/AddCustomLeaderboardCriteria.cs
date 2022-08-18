using DevilDaggersInfo.Types.Web;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public record AddCustomLeaderboardCriteria
{
	public CustomLeaderboardCriteriaOperator Operator { get; set; }

	public string? Expression { get; set; }

	// TODO: Remove.
	public int Value { get; set; }
}
