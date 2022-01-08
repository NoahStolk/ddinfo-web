namespace DevilDaggersInfo.SourceGen.Core.Wiki.Generators;

[Generator]
public class DeathSourceGenerator : IIncrementalGenerator
{
	private const string _className = $"%{nameof(_className)}%";
	private const string _deathFields = $"%{nameof(_deathFields)}%";
	private const string _template = $@"namespace DevilDaggersInfo.Core.Wiki;

public static class {_className}
{{
{_deathFields}

	internal static readonly List<Death> All = typeof({_className}).GetFields().Where(f => f.FieldType == typeof(Death)).Select(f => (Death)f.GetValue(null)!).ToList();
}}
";

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		IncrementalValuesProvider<AdditionalText> additionalTexts = context.AdditionalTextsProvider.Where(static at => Path.GetFileName(at.Path).StartsWith("Deaths"));

		context.RegisterSourceOutput(additionalTexts, static (spc, at) => Execute(spc, at));
	}

	private static void Execute(SourceProductionContext sourceProductionContext, AdditionalText additionalText)
	{
		string gameVersion = Path.GetFileNameWithoutExtension(additionalText.Path).Replace("Deaths", string.Empty);
		string className = $"Deaths{gameVersion}";

		string? fileContents = additionalText.GetText()?.ToString();
		if (fileContents == null)
			return;

		string[] lines = fileContents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
		string[] fieldLines = new string[lines.Length];

		for (int i = 0; i < lines.Length; i++)
		{
			string line = lines[i];

			string[] parameters = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
			const int parameterCount = 3;
			if (parameters.Length != parameterCount)
				throw new($"Invalid specification in line '{line}'. There should be {parameterCount} parameters, but {parameters.Length} were found.");

			fieldLines[i] = $"public static readonly Death {parameters[0]} = new(GameVersion.{gameVersion}, \"{parameters[0].ToUpper()}\", EnemyColors.{parameters[1]}, {parameters[2]});";
		}

		string source = _template
			.Replace(_className, className)
			.Replace(_deathFields, string.Join(Environment.NewLine, fieldLines).IndentCode(1));
		sourceProductionContext.AddSource(className, SourceText.From(source.WrapCodeInsideWarningSuppressionDirectives().TrimCode(), Encoding.UTF8));
	}
}
