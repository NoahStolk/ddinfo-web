using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Core.CriteriaExpression.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevilDaggersInfo.Web.Core.CriteriaExpression.Tests;

[TestClass]
public class ExpressionValidationTests
{
	[TestMethod]
	public void TestValidExpressions()
	{
		TestExpression(new() { new ExpressionValue(1), new ExpressionOperator(ExpressionOperatorType.Subtract), new ExpressionTarget(CustomLeaderboardCriteriaType.GemsCollected) });
		TestExpression(new() { new ExpressionValue(180), new ExpressionOperator(ExpressionOperatorType.Add), new ExpressionTarget(CustomLeaderboardCriteriaType.GemsCollected), new ExpressionOperator(ExpressionOperatorType.Subtract), new ExpressionTarget(CustomLeaderboardCriteriaType.DaggersFired) });
		TestExpression(new() { new ExpressionValue(10), new ExpressionOperator(ExpressionOperatorType.Add), new ExpressionValue(5) });
		TestExpression(new() { new ExpressionValue(20) });
	}

	[TestMethod]
	public void TestInvalidExpressions()
	{
		Assert.ThrowsException<CriteriaExpressionParseException>(() => TestExpression(new() { new ExpressionValue(1), new ExpressionOperator(ExpressionOperatorType.Subtract) }));
		Assert.ThrowsException<CriteriaExpressionParseException>(() => TestExpression(new() { new ExpressionOperator(ExpressionOperatorType.Add), new ExpressionValue(5), new ExpressionValue(10) }));
		Assert.ThrowsException<CriteriaExpressionParseException>(() => TestExpression(new() { new ExpressionValue(20), new ExpressionValue(20) }));
		Assert.ThrowsException<CriteriaExpressionParseException>(() => TestExpression(new() { new ExpressionOperator(ExpressionOperatorType.Subtract) }));
		Assert.ThrowsException<CriteriaExpressionParseException>(() => TestExpression(new()));
	}

	private static void TestExpression(List<IExpressionPart> parts)
	{
		Expression expression = new(parts);
		expression.Validate();
	}
}
