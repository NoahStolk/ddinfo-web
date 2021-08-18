namespace DevilDaggersInfo.Core.Wiki.SourceGenerator.Utils;

public static class SourceBuilderUtils
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

	public static string WrapInsideWarningSuppressionDirectives(string source)
	{
		string warningSuppressionCodes = string.Join(", ", _suppressedErrorCodes);
		string warningsDisable = $"#pragma warning disable {warningSuppressionCodes}\n";
		string warningsRestore = $"\n#pragma warning restore {warningSuppressionCodes}";
		return warningsDisable + source + warningsRestore;
	}
}
