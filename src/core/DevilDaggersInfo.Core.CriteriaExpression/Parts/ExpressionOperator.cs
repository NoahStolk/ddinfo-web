using System.Diagnostics;

namespace DevilDaggersInfo.Core.CriteriaExpression.Parts;

public record ExpressionOperator(ExpressionOperatorType Operator) : IExpressionPart
{
	public override string ToString() => Operator switch
	{
		ExpressionOperatorType.Add => "+",
		ExpressionOperatorType.Subtract => "-",
		_ => throw new UnreachableException(),
	};
}
