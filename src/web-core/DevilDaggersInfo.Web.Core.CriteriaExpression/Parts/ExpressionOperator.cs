using DevilDaggersInfo.Common.Exceptions;

namespace DevilDaggersInfo.Web.Core.CriteriaExpression.Parts;

public record ExpressionOperator(ExpressionOperatorType Operator) : IExpressionPart
{
	public override string ToString()
	{
		return Operator switch
		{
			ExpressionOperatorType.Add => "+",
			ExpressionOperatorType.Subtract => "-",
			_ => throw new InvalidEnumConversionException(Operator),
		};
	}
}
