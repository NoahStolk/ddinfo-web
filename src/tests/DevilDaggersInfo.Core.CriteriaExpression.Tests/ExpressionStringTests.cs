using DevilDaggersInfo.Core.CriteriaExpression.Parts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevilDaggersInfo.Core.CriteriaExpression.Tests;

[TestClass]
public class ExpressionStringTests
{
	[TestMethod]
	public void TestStringConversions()
	{
		TestExpression("1 - Gems collected", new() { new ExpressionValue(1), new ExpressionOperator(ExpressionOperatorType.Subtract), new ExpressionTarget(CustomLeaderboardCriteriaType.GemsCollected) });
		TestExpression("180 + Gems collected - Daggers fired", new() { new ExpressionValue(180), new ExpressionOperator(ExpressionOperatorType.Add), new ExpressionTarget(CustomLeaderboardCriteriaType.GemsCollected), new ExpressionOperator(ExpressionOperatorType.Subtract), new ExpressionTarget(CustomLeaderboardCriteriaType.DaggersFired) });
		TestExpression("10 + 5", new() { new ExpressionValue(10), new ExpressionOperator(ExpressionOperatorType.Add), new ExpressionValue(5) });
		TestExpression("20", new() { new ExpressionValue(20) });

		static void TestExpression(string expectedString, List<IExpressionPart> parts)
		{
			Expression expression = new(parts);
			expression.Validate();

			Assert.AreEqual(expectedString, expression.ToString());
			Assert.IsTrue(ContainsSameParts(expression, Expression.Parse(expectedString)));
		}

		static bool ContainsSameParts(Expression a, Expression b)
		{
			if (a.Parts.Count != b.Parts.Count)
				return false;

			for (int i = 0; i < a.Parts.Count; i++)
			{
				// Cannot compare directly because instances of IExpressionPart will never be equal (reference comparison). Casting to the relevant records is required for the equality contracts to work.
				IExpressionPart aPart = a.Parts[i];
				IExpressionPart bPart = b.Parts[i];

				switch (aPart)
				{
					case ExpressionValue aValue when bPart is not ExpressionValue bValue || aValue != bValue:
					case ExpressionOperator aOperator when bPart is not ExpressionOperator bOperator || aOperator != bOperator:
					case ExpressionTarget aTarget when bPart is not ExpressionTarget bTarget || aTarget != bTarget:
						return false;
				}
			}

			return true;
		}
	}
}
