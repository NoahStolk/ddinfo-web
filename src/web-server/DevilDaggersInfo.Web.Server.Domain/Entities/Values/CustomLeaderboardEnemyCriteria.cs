using DevilDaggersInfo.Types.Web;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Entities.Values;

[Owned]
public class CustomLeaderboardEnemyCriteria
{
	public CustomLeaderboardCriteriaOperator Operator { get; set; }

	public short Value { get; set; }
}
