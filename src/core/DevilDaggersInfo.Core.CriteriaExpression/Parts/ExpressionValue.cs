using DevilDaggersInfo.Common;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Objects;
using DevilDaggersInfo.Types.Core.CustomLeaderboards;

namespace DevilDaggersInfo.Core.CriteriaExpression.Parts;

public record ExpressionValue(int Value) : IExpressionPart
{
	public override string ToString()
	{
		return Value.ToString();
	}

	public string ToDisplayString(CustomLeaderboardCriteriaType criteriaType)
	{
		if (criteriaType == CustomLeaderboardCriteriaType.DeathType)
		{
			Death? death = Deaths.GetDeathByLeaderboardType(GameConstants.CurrentVersion, (byte)Value);
			return death.HasValue ? death.Value.Name : "???";
		}

		bool isTime = criteriaType is CustomLeaderboardCriteriaType.Time or CustomLeaderboardCriteriaType.LevelUpTime2 or CustomLeaderboardCriteriaType.LevelUpTime3 or CustomLeaderboardCriteriaType.LevelUpTime4;
		return isTime ? Value.ToSecondsTime().ToString(StringFormats.TimeFormat) : Value.ToString();
	}
}
