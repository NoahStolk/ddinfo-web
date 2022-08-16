using DevilDaggersInfo.Types.Web;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Entities.Values;

// TODO: Use generic math.
[Owned]
public class CustomLeaderboardCriteriaEntityValue
{
	public CustomLeaderboardCriteriaOperator Operator { get; set; }

	public int Value { get; set; }
}
