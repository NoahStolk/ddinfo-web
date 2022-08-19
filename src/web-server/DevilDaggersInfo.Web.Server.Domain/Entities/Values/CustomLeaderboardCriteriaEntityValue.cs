using DevilDaggersInfo.Types.Web;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Entities.Values;

[Owned]
public class CustomLeaderboardCriteriaEntityValue
{
	public CustomLeaderboardCriteriaOperator Operator { get; init; }

	// TODO: Remove.
	public int Value { get; set; }

	[MaxLength(Core.CriteriaExpression.Expression.MaxByteLength)]
	public byte[]? Expression { get; init; }
}
