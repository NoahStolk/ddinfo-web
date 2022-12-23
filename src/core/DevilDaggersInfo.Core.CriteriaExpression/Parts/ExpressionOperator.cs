using DevilDaggersInfo.Common.Exceptions;

namespace DevilDaggersInfo.Core.CriteriaExpression.Parts;

public record ExpressionOperator(ExpressionOperatorType Operator) : IExpressionPart
{
	public override string ToString() => Operator switch
	{
		ExpressionOperatorType.Add => "+",
		ExpressionOperatorType.Subtract => "-",
		_ => throw new InvalidEnumConversionException(Operator),
	};
}
