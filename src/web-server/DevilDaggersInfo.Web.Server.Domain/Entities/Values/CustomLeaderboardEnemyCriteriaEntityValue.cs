using DevilDaggersInfo.Types.Web;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Entities.Values;

// TODO: Remove.
[Owned]
public class CustomLeaderboardEnemyCriteriaEntityValue
{
	public CustomLeaderboardCriteriaOperator Operator { get; init; }

	// TODO: Remove.
	public short Value { get; set; }

	[MaxLength(Core.CriteriaExpression.Expression.MaxByteLength)]
	public byte[]? Expression { get; init; }
}
