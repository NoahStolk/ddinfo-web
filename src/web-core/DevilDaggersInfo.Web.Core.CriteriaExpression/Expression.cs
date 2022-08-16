using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Core.CriteriaExpression.Exceptions;
using DevilDaggersInfo.Web.Core.CriteriaExpression.Parts;

namespace DevilDaggersInfo.Web.Core.CriteriaExpression;

public class Expression
{
	public Expression(List<IExpressionPart> parts)
	{
		Parts = parts;
	}

	public List<IExpressionPart> Parts { get; }

	public static Expression TryParse(byte[] bytes)
	{
		try
		{
			return Parse(bytes);
		}
		catch (Exception ex) when (ex is not CriteriaExpressionParseException)
		{
			throw new CriteriaExpressionParseException("An unhandled exception occurred while trying to parse the criteria expression.", ex);
		}
	}

	private static Expression Parse(byte[] bytes)
	{
		List<IExpressionPart> parts = new();

		using MemoryStream ms = new(bytes);
		using BinaryReader br = new(ms);

		byte version = br.ReadByte();
		if (version != 0)
			throw new CriteriaExpressionParseException($"Criteria expression version '{version}' is not supported.");

		while (br.BaseStream.Position < br.BaseStream.Length)
		{
			byte partType = br.ReadByte();
			parts.Add(partType switch
			{
				0x00 => new ExpressionOperator((ExpressionOperatorType)br.ReadByte()),
				0x01 => new ExpressionTarget((CustomLeaderboardCriteriaType)br.ReadByte()),
				0x02 => new ExpressionValue(br.ReadInt32()),
				_ => throw new CriteriaExpressionParseException($"Criteria expression part type '{partType}' is not supported."),
			});
		}

		return new(parts);
	}

	public void Validate()
	{
		if (Parts.Count % 2 == 0)
			throw new CriteriaExpressionParseException("Expression must consist of an uneven amount of parts.");

		for (int i = 0; i < Parts.Count; i++)
		{
			IExpressionPart part = Parts[i];
			if (i % 2 != 0 && part is not ExpressionOperator)
				throw new CriteriaExpressionParseException("Invalid expression.");
			else if (i % 2 == 0 && part is ExpressionOperator)
				throw new CriteriaExpressionParseException("Invalid expression.");
		}
	}

	public int Evaluate(TargetCollection targetCollection)
	{
		int result = EvaluatePart(Parts[0]);

		for (int i = 0; i < Parts.Count - 2; i += 2)
		{
			ExpressionOperator op = (Parts[i + 1] as ExpressionOperator) ?? throw new InvalidOperationException("Invalid expression.");
			int right = EvaluatePart(Parts[i + 2]);

			result = op.Operator switch
			{
				ExpressionOperatorType.Add => result + right,
				ExpressionOperatorType.Subtract => result - right,
				_ => throw new InvalidEnumConversionException(op.Operator),
			};
		}

		return result;

		int EvaluatePart(IExpressionPart part)
		{
			if (part is ExpressionValue value)
				return value.Value;

			if (part is not ExpressionTarget target)
				throw new InvalidOperationException("Invalid expression.");

			return target.Target switch
			{
				CustomLeaderboardCriteriaType.GemsCollected => targetCollection.GemsCollected,
				CustomLeaderboardCriteriaType.GemsDespawned => targetCollection.GemsDespawned,
				CustomLeaderboardCriteriaType.GemsEaten => targetCollection.GemsEaten,
				CustomLeaderboardCriteriaType.EnemiesKilled => targetCollection.EnemiesKilled,
				CustomLeaderboardCriteriaType.DaggersFired => targetCollection.DaggersFired,
				CustomLeaderboardCriteriaType.DaggersHit => targetCollection.DaggersHit,
				CustomLeaderboardCriteriaType.HomingStored => targetCollection.HomingStored,
				CustomLeaderboardCriteriaType.HomingEaten => targetCollection.HomingEaten,
				CustomLeaderboardCriteriaType.Skull1Kills => targetCollection.Skull1Kills,
				CustomLeaderboardCriteriaType.Skull2Kills => targetCollection.Skull2Kills,
				CustomLeaderboardCriteriaType.Skull3Kills => targetCollection.Skull3Kills,
				CustomLeaderboardCriteriaType.Skull4Kills => targetCollection.Skull4Kills,
				CustomLeaderboardCriteriaType.SpiderlingKills => targetCollection.SpiderlingKills,
				CustomLeaderboardCriteriaType.SpiderEggKills => targetCollection.SpiderEggKills,
				CustomLeaderboardCriteriaType.Squid1Kills => targetCollection.Squid1Kills,
				CustomLeaderboardCriteriaType.Squid2Kills => targetCollection.Squid2Kills,
				CustomLeaderboardCriteriaType.Squid3Kills => targetCollection.Squid3Kills,
				CustomLeaderboardCriteriaType.CentipedeKills => targetCollection.CentipedeKills,
				CustomLeaderboardCriteriaType.GigapedeKills => targetCollection.GigapedeKills,
				CustomLeaderboardCriteriaType.GhostpedeKills => targetCollection.GhostpedeKills,
				CustomLeaderboardCriteriaType.Spider1Kills => targetCollection.Spider1Kills,
				CustomLeaderboardCriteriaType.Spider2Kills => targetCollection.Spider2Kills,
				CustomLeaderboardCriteriaType.LeviathanKills => targetCollection.LeviathanKills,
				CustomLeaderboardCriteriaType.OrbKills => targetCollection.OrbKills,
				CustomLeaderboardCriteriaType.ThornKills => targetCollection.ThornKills,
				_ => throw new InvalidEnumConversionException(target.Target),
			};
		}
	}
}
