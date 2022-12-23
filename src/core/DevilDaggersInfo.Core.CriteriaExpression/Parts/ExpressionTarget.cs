using DevilDaggersInfo.Types.Core.CustomLeaderboards;
using DevilDaggersInfo.Types.Core.CustomLeaderboards.Extensions;

namespace DevilDaggersInfo.Core.CriteriaExpression.Parts;

public record ExpressionTarget(CustomLeaderboardCriteriaType Target) : IExpressionPart
{
	public override string ToString()
	{
		return Target.Display();
	}
}
