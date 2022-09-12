using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Core.CriteriaExpression.Parts;

public record ExpressionValue(int Value) : IExpressionPart
{
	public override string ToString()
	{
		return Value.ToString();
	}

	public string ToDisplayString(CustomLeaderboardCriteriaType criteriaType)
	{
		bool isTime = criteriaType is CustomLeaderboardCriteriaType.Time or CustomLeaderboardCriteriaType.LevelUpTime2 or CustomLeaderboardCriteriaType.LevelUpTime3 or CustomLeaderboardCriteriaType.LevelUpTime4;
		return isTime ? Value.ToSecondsTime().ToString() : Value.ToString();
	}
}
