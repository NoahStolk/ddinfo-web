using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Core.CriteriaExpression;

public record ExpressionTarget(CustomLeaderboardCriteriaType Target) : IExpressionPart;
