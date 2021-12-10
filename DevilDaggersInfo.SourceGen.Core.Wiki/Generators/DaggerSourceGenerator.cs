namespace DevilDaggersInfo.SourceGen.Core.Wiki.Generators;

[Generator]
public class DaggerSourceGenerator : IIncrementalGenerator
{
	private const string _className = $"%{nameof(_className)}%";
	private const string _daggerFields = $"%{nameof(_daggerFields)}%";
	private const string _template = $@"namespace DevilDaggersInfo.Core.Wiki;

public static class {_className}
{{
{_daggerFields}

	internal static readonly List<Dagger> All = typeof({_className}).GetFields().Where(f => f.FieldType == typeof(Dagger)).Select(f => (Dagger)f.GetValue(null)!).ToList();
}}
";

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		IncrementalValuesProvider<AdditionalText> additionalTexts = context.AdditionalTextsProvider.Where(static at => Path.GetFileName(at.Path).StartsWith("Daggers"));

		context.RegisterSourceOutput(additionalTexts, static (spc, at) => Execute(spc, at));
	}

	public static void Execute(SourceProductionContext sourceProductionContext, AdditionalText additionalText)
	{
		string gameVersion = Path.GetFileNameWithoutExtension(additionalText.Path).Replace("Daggers", string.Empty);
		string className = $"Daggers{gameVersion}";

		string? fileContents = additionalText.GetText()?.ToString();
		if (fileContents == null)
			return;

		string[] lines = fileContents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
		string[] fieldLines = new string[lines.Length];

		for (int i = 0; i < lines.Length; i++)
		{
			string line = lines[i];

			string[] parameters = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
			const int parameterCount = 2;
			if (parameters.Length != parameterCount)
				throw new($"Invalid specification in line '{line}'. There should be {parameterCount} parameters, but {parameters.Length} were found.");

			fieldLines[i] = $"public static readonly Dagger {parameters[0]} = new(GameVersion.{gameVersion}, \"{parameters[0]}\", DaggerColors.{parameters[0]}, {parameters[1]});";
		}

		string source = _template
			.Replace(_className, className)
			.Replace(_daggerFields, string.Join(Environment.NewLine, fieldLines).Indent(1));
		sourceProductionContext.AddSource(className, SourceText.From(SourceBuilderUtils.WrapInsideWarningSuppressionDirectives(source), Encoding.UTF8));
	}
}
