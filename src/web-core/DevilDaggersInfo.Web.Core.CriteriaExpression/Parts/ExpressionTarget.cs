using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Types.Web.Extensions;

namespace DevilDaggersInfo.Web.Core.CriteriaExpression.Parts;

public record ExpressionTarget(CustomLeaderboardCriteriaType Target) : IExpressionPart
{
	public override string ToString()
	{
		return Target.ToStringFast();
	}
}
