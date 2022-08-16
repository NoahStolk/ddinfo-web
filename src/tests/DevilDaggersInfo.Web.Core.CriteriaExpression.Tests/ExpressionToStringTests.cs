using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Core.CriteriaExpression.Parts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevilDaggersInfo.Web.Core.CriteriaExpression.Tests;

[TestClass]
public class ExpressionToStringTests
{
	[TestMethod]
	public void TestValidExpressions()
	{
		TestExpression("1 - GemsCollected", new() { new ExpressionValue(1), new ExpressionOperator(ExpressionOperatorType.Subtract), new ExpressionTarget(CustomLeaderboardCriteriaType.GemsCollected) });
		TestExpression("180 + GemsCollected - DaggersFired", new() { new ExpressionValue(180), new ExpressionOperator(ExpressionOperatorType.Add), new ExpressionTarget(CustomLeaderboardCriteriaType.GemsCollected), new ExpressionOperator(ExpressionOperatorType.Subtract), new ExpressionTarget(CustomLeaderboardCriteriaType.DaggersFired) });
		TestExpression("10 + 5", new() { new ExpressionValue(10), new ExpressionOperator(ExpressionOperatorType.Add), new ExpressionValue(5) });
		TestExpression("20", new() { new ExpressionValue(20) });
	}

	private static void TestExpression(string expectedString, List<IExpressionPart> parts)
	{
		Expression expression = new(parts);
		expression.Validate();

		Assert.AreEqual(expectedString, expression.ToString());
	}
}
