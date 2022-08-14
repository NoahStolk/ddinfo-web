using DevilDaggersInfo.Types.Web;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public record AddCustomLeaderboardEnemyCriteria
{
	public CustomLeaderboardCriteriaOperator Operator { get; set; }

	[Range(0, short.MaxValue)]
	public short Value { get; set; }
}
