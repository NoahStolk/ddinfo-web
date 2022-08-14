using DevilDaggersInfo.Types.Web;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Entities.Values;

[Owned]
public class CustomLeaderboardCriteria
{
	public CustomLeaderboardCriteriaOperator Operator { get; set; }

	public int Value { get; set; }
}
