using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Core.CriteriaExpression.Parts;

public record ExpressionTarget(CustomLeaderboardCriteriaType Target) : IExpressionPart
{
	public override string ToString()
	{
		return Target.ToString();
	}
}
