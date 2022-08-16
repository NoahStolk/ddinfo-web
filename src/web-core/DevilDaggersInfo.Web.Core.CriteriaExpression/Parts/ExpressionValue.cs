namespace DevilDaggersInfo.Web.Core.CriteriaExpression.Parts;

public record ExpressionValue(int Value) : IExpressionPart
{
	public override string ToString()
	{
		return Value.ToString();
	}
}
