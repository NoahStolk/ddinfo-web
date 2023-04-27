using DevilDaggersInfo.Core.CriteriaExpression.Exceptions;
using DevilDaggersInfo.Core.CriteriaExpression.Parts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevilDaggersInfo.Core.CriteriaExpression.Tests;

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
		TestExpression(new() { new ExpressionTarget(CustomLeaderboardCriteriaType.GemsCollected) });
	}

	[TestMethod]
	public void TestInvalidExpressions()
	{
		Assert.ThrowsException<CriteriaExpressionParseException>(() => TestExpression(new() { new ExpressionValue(1), new ExpressionOperator(ExpressionOperatorType.Subtract) }));
		Assert.ThrowsException<CriteriaExpressionParseException>(() => TestExpression(new() { new ExpressionOperator(ExpressionOperatorType.Add), new ExpressionValue(5), new ExpressionValue(10) }));
		Assert.ThrowsException<CriteriaExpressionParseException>(() => TestExpression(new() { new ExpressionValue(20), new ExpressionValue(20) }));
		Assert.ThrowsException<CriteriaExpressionParseException>(() => TestExpression(new() { new ExpressionOperator(ExpressionOperatorType.Subtract) }));
		Assert.ThrowsException<CriteriaExpressionParseException>(() => TestExpression(new()));
		Assert.ThrowsException<CriteriaExpressionParseException>(() => TestExpression(new() { new ExpressionTarget(CustomLeaderboardCriteriaType.Time), new ExpressionOperator(ExpressionOperatorType.Add), new ExpressionValue(5) }));
		Assert.ThrowsException<CriteriaExpressionParseException>(() => TestExpression(new() { new ExpressionTarget(CustomLeaderboardCriteriaType.Time) }));
		Assert.ThrowsException<CriteriaExpressionParseException>(() => TestExpression(new() { new ExpressionTarget(CustomLeaderboardCriteriaType.DeathType) }));
		Assert.ThrowsException<CriteriaExpressionParseException>(() => TestExpression(new() { new ExpressionTarget(CustomLeaderboardCriteriaType.LevelUpTime2) }));
	}

	private static void TestExpression(List<IExpressionPart> parts)
	{
		Expression expression = new(parts);
		expression.Validate();
	}
}
