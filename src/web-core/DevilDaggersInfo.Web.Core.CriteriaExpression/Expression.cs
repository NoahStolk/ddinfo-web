using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Core.CriteriaExpression.Exceptions;

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
}
