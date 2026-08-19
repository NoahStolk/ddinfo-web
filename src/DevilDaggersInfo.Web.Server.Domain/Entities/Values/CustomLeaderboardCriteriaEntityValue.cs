using DevilDaggersInfo.Core.CriteriaExpression;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Entities.Values;

[Owned]
public sealed class CustomLeaderboardCriteriaEntityValue
{
	public CustomLeaderboardCriteriaOperator Operator { get; init; }

	[MaxLength(DevilDaggersInfo.Core.CriteriaExpression.Expression.MaxByteLength)]
	public byte[]? Expression { get; init; }
}
