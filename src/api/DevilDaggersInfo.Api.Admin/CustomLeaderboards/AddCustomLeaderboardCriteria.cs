using DevilDaggersInfo.Types.Web;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public record AddCustomLeaderboardCriteria
{
	public CustomLeaderboardCriteriaOperator Operator { get; set; }

	[Range(0, int.MaxValue)]
	public int Value { get; set; }
}
