namespace DevilDaggersInfo.SourceGen.Core.Wiki.Generators;

[Generator]
public class UpgradeSourceGenerator : IIncrementalGenerator
{
	private const string _className = $"%{nameof(_className)}%";
	private const string _upgradeFields = $"%{nameof(_upgradeFields)}%";
	private const string _template = $@"namespace DevilDaggersInfo.Core.Wiki;

public static class {_className}
{{
{_upgradeFields}

	internal static readonly List<Upgrade> All = typeof({_className}).GetFields().Where(f => f.FieldType == typeof(Upgrade)).Select(f => (Upgrade)f.GetValue(null)!).ToList();
}}
";

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		IncrementalValuesProvider<AdditionalText> additionalTexts = context.AdditionalTextsProvider.Where(static at => Path.GetFileName(at.Path).StartsWith("Upgrades"));

		context.RegisterSourceOutput(additionalTexts, static (spc, at) => Execute(spc, at));
	}

	private static void Execute(SourceProductionContext sourceProductionContext, AdditionalText additionalText)
	{
		string gameVersion = Path.GetFileNameWithoutExtension(additionalText.Path).Replace("Upgrades", string.Empty);
		string className = $"Upgrades{gameVersion}";

		string? fileContents = additionalText.GetText()?.ToString();
		if (fileContents == null)
			return;

		string[] lines = fileContents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
		string[] fieldLines = new string[lines.Length];

		for (int i = 0; i < lines.Length; i++)
		{
			string line = lines[i];

			string[] parameters = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
			const int parameterCount = 7;
			if (parameters.Length != parameterCount)
				throw new($"Invalid specification in line '{line}'. There should be {parameterCount} parameters, but {parameters.Length} were found.");

			int level = int.Parse(parameters[0]);
			int defaultShot = int.Parse(parameters[1]);
			float defaultSpray = float.Parse(parameters[2]);
			int? homingShot = parameters[3] == "!" ? null : int.Parse(parameters[3]);
			float? homingSpray = parameters[4] == "!" ? null : float.Parse(parameters[4]);
			string unlockType = parameters[5];
			int unlockValue = int.Parse(parameters[6]);
			fieldLines[i] = $"public static readonly Upgrade Level{level} = new(GameVersion.{gameVersion}, \"Level {level}\", UpgradeColors.Level{level}, {level}, new({defaultShot}, {defaultSpray}f), new({ValueOrNull(homingShot)}, {ValueOrNull(homingSpray)}), new(UpgradeUnlockType.{unlockType}, {unlockValue}));";
		}

		string source = _template
			.Replace(_className, className)
			.Replace(_upgradeFields, string.Join(Environment.NewLine, fieldLines).Indent(1));
		sourceProductionContext.AddSource(className, SourceText.From(SourceBuilderUtils.WrapInsideWarningSuppressionDirectives(source), Encoding.UTF8));
	}

	private static string ValueOrNull(float? value)
		=> value.HasValue ? $"{value.Value}f" : "null";

	private static string ValueOrNull(int? value)
		=> value.HasValue ? value.Value.ToString() : "null";
}
