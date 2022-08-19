using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Types.Web.Extensions;
using DevilDaggersInfo.Web.Core.CriteriaExpression.Exceptions;
using DevilDaggersInfo.Web.Core.CriteriaExpression.Parts;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace DevilDaggersInfo.Web.Core.CriteriaExpression;

public class Expression
{
	public const int MaxByteLength = 64;

	public Expression(List<IExpressionPart> parts)
	{
		// TODO: We probably want to remove any redundant operations, such as 10 + 5 which could be converted to 15.
		Parts = parts;
		Validate();
	}

	public List<IExpressionPart> Parts { get; }

	public static bool TryParse(string str, [NotNullWhen(true)] out Expression? expression)
	{
		try
		{
			expression = Parse(str);
			return true;
		}
		catch
		{
			expression = null;
			return false;
		}
	}

	public static bool TryParse(byte[] bytes, [NotNullWhen(true)] out Expression? expression)
	{
		try
		{
			expression = Parse(bytes);
			return true;
		}
		catch
		{
			expression = null;
			return false;
		}
	}

	public static Expression Parse(string str)
	{
		str = str.Replace(" ", string.Empty);

		List<string> delimiters = new() { "+", "-" };
		string pattern = "(" + string.Join("|", delimiters.Select(Regex.Escape).ToArray()) + ")";
		string[] result = Regex.Split(str, pattern);
		return new(Array.ConvertAll(result, Parse).ToList());

		static IExpressionPart Parse(string str)
		{
			if (str.Length == 0)
				throw new CriteriaExpressionParseException("Empty expression part.");

			if (int.TryParse(str, out int value))
				return new ExpressionValue(value);

			foreach (CustomLeaderboardCriteriaType criteriaType in Enum.GetValues<CustomLeaderboardCriteriaType>())
			{
				if (string.Equals(str, criteriaType.GetIdentifier(), StringComparison.OrdinalIgnoreCase) || string.Equals(str, criteriaType.ToStringFast(), StringComparison.OrdinalIgnoreCase))
					return new ExpressionTarget(criteriaType);
			}

			return str switch
			{
				"+" => new ExpressionOperator(ExpressionOperatorType.Add),
				"-" => new ExpressionOperator(ExpressionOperatorType.Subtract),
				_ => throw new CriteriaExpressionParseException($"Invalid expression part '{str}'."),
			};
		}
	}

	public static Expression Parse(byte[] bytes)
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

	public override string ToString()
	{
		return string.Join(" ", Parts);
	}

	public string ToShortString()
	{
		StringBuilder sb = new();
		foreach (IExpressionPart part in Parts)
		{
			switch (part)
			{
				case ExpressionOperator op: sb.Append(op); break;
				case ExpressionTarget target: sb.Append(target.Target.GetIdentifier()); break;
				case ExpressionValue value: sb.Append(value); break;
				default: throw new CriteriaExpressionParseException($"Criteria expression part type '{part.GetType().Name}' is not supported.");
			}
		}

		return sb.ToString();
	}

	public byte[] ToBytes()
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		bw.Write((byte)0x00); // Version

		foreach (IExpressionPart part in Parts)
		{
			switch (part)
			{
				case ExpressionOperator op: WriteOperator(bw, op); break;
				case ExpressionTarget target: WriteTarget(bw, target); break;
				case ExpressionValue value: WriteValue(bw, value); break;
				default: throw new CriteriaExpressionParseException($"Criteria expression part type '{part.GetType().Name}' is not supported.");
			}
		}

		static void WriteOperator(BinaryWriter bw, ExpressionOperator op)
		{
			bw.Write((byte)0x00);
			bw.Write((byte)op.Operator);
		}

		static void WriteTarget(BinaryWriter bw, ExpressionTarget target)
		{
			bw.Write((byte)0x01);
			bw.Write((byte)target.Target);
		}

		static void WriteValue(BinaryWriter bw, ExpressionValue value)
		{
			bw.Write((byte)0x02);
			bw.Write(value.Value);
		}

		return ms.ToArray();
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
			if (i % 2 == 0 && part is ExpressionOperator)
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
