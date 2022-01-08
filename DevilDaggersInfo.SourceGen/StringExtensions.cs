using System.Text;

namespace DevilDaggersInfo.SourceGen;

public static class StringExtensions
{
	private static readonly string[] _suppressedErrorCodes = new string[]
	{
		"CS0105",
		"CS1591",
		"CS8618",
		"S1128",
		"SA1001",
		"SA1027",
		"SA1028",
		"SA1101",
		"SA1122",
		"SA1137",
		"SA1200",
		"SA1201",
		"SA1208",
		"SA1210",
		"SA1309",
		"SA1311",
		"SA1413",
		"SA1503",
		"SA1505",
		"SA1507",
		"SA1508",
		"SA1516",
		"SA1600",
		"SA1601",
		"SA1602",
		"SA1623",
		"SA1649",
	};

	public static string WrapCodeInsideWarningSuppressionDirectives(this string code)
	{
		string warningSuppressionCodes = string.Join(", ", _suppressedErrorCodes);
		string warningsDisable = $"#pragma warning disable {warningSuppressionCodes}{Environment.NewLine}";
		string warningsRestore = $"{Environment.NewLine}#pragma warning restore {warningSuppressionCodes}";
		return warningsDisable + code + warningsRestore;
	}

	public static string IndentCode(this string code, int count)
	{
		string indentation = new('\t', count);
		return code.Insert(0, indentation).Replace(Environment.NewLine, Environment.NewLine + indentation);
	}

	public static string TrimCode(this string code)
	{
		StringBuilder sb = new();
		foreach (string line in code.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
			sb.AppendLine(line.TrimEnd());

		return sb.ToString();
	}

	public static string TrimStart(this string str, params string[] values)
	{
		if (values.Length == 0)
			return str;

		string? sub = Array.Find(values, v => str.StartsWith(v));
		return sub == null ? str : str.Substring(sub.Length);
	}
}
