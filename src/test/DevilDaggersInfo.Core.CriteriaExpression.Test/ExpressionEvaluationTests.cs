using DevilDaggersInfo.Core.CriteriaExpression.Parts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevilDaggersInfo.Core.CriteriaExpression.Tests;

[TestClass]
public class ExpressionEvaluationTests
{
	[TestMethod]
	public void TestValidExpressions()
	{
		TargetCollection tc = new()
		{
			GemsCollected = 100,
			GemsDespawned = 10,
			GemsEaten = 15,
			EnemiesKilled = 170,
			DaggersFired = 1500,
			DaggersHit = 1000,
			HomingStored = 30,
			HomingEaten = 5,
			Skull1Kills = 100,
			Skull2Kills = 10,
			Skull3Kills = 5,
			Skull4Kills = 2,
			SpiderlingKills = 10,
			SpiderEggKills = 5,
			Squid1Kills = 5,
			Squid2Kills = 5,
			Squid3Kills = 5,
			CentipedeKills = 1,
			GigapedeKills = 1,
			GhostpedeKills = 1,
			Spider1Kills = 10,
			Spider2Kills = 5,
			LeviathanKills = 1,
			OrbKills = 1,
			ThornKills = 8,
		};

		TestExpression(200, tc, new() { new ExpressionValue(100), new ExpressionOperator(ExpressionOperatorType.Add), new ExpressionTarget(CustomLeaderboardCriteriaType.GemsCollected) });
		TestExpression(55, tc, new() { new ExpressionTarget(CustomLeaderboardCriteriaType.GemsCollected), new ExpressionOperator(ExpressionOperatorType.Subtract), new ExpressionTarget(CustomLeaderboardCriteriaType.HomingStored), new ExpressionOperator(ExpressionOperatorType.Subtract), new ExpressionTarget(CustomLeaderboardCriteriaType.GemsEaten) });
		TestExpression(15, tc, new() { new ExpressionValue(10), new ExpressionOperator(ExpressionOperatorType.Add), new ExpressionValue(5) });
		TestExpression(20, tc, new() { new ExpressionValue(20) });
	}

	private static void TestExpression(int expectedResult, TargetCollection tc, List<IExpressionPart> parts)
	{
		Expression expression = new(parts);
		expression.Validate();

		Assert.AreEqual(expectedResult, expression.Evaluate(tc));
	}
}
