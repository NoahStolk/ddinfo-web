namespace DevilDaggersInfo.Core.Wiki.SourceGenerator;

[Generator]
public class DaggerSourceGenerator : ISourceGenerator
{
	private const string _className = $"%{nameof(_className)}%";
	private const string _daggerFields = $"%{nameof(_daggerFields)}%";
	private const string _template = $@"namespace DevilDaggersInfo.Core.Wiki;

public static class {_className}
{{
{_daggerFields}

	public static readonly List<Dagger> All = typeof({_className}).GetFields().Where(f => f.FieldType == typeof(Dagger)).Select(f => (Dagger)f.GetValue(null)!).ToList();
}}
";

	public void Initialize(GeneratorInitializationContext context)
	{
		// Method intentionally left empty.
	}

	public void Execute(GeneratorExecutionContext context)
	{
		foreach (AdditionalText additionalText in context.AdditionalFiles.Where(at => Path.GetFileNameWithoutExtension(at.Path).StartsWith("Daggers")))
		{
			string gameVersion = Path.GetFileNameWithoutExtension(additionalText.Path).Replace("Daggers", string.Empty);
			string className = $"Daggers{gameVersion}";

			string? fileContents = additionalText.GetText()?.ToString();
			if (fileContents == null)
				continue;

			string[] lines = fileContents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			string[] fieldLines = new string[lines.Length];

			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];

				string[] parameters = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
				const int parameterCount = 2;
				if (parameters.Length != parameterCount)
					throw new($"Invalid specification in line '{line}'. There should be {parameterCount} parameters, but {parameters.Length} were found.");

				fieldLines[i] = $"\tpublic static readonly Dagger {parameters[0]} = new(GameVersion.{gameVersion}, \"{parameters[0]}\", DaggerColors.{parameters[0]}, {parameters[1]});";
			}

			string sourceBuilder = _template
				.Replace(_className, className)
				.Replace(_daggerFields, string.Join(Environment.NewLine, fieldLines));

			string warningSuppressionCodes = string.Join(", ", new[] { "CS0105", "CS1591", "CS8618", "S1128", "SA1001", "SA1027", "SA1028", "SA1101", "SA1122", "SA1137", "SA1200", "SA1201", "SA1208", "SA1210", "SA1309", "SA1311", "SA1413", "SA1503", "SA1505", "SA1507", "SA1508", "SA1516", "SA1600", "SA1601", "SA1602", "SA1623", "SA1649" });
			string warningsDisable = $"#pragma warning disable {warningSuppressionCodes}\n";
			string warningsRestore = $"\n#pragma warning restore {warningSuppressionCodes}";
			context.AddSource(className, SourceText.From(warningsDisable + sourceBuilder + warningsRestore, Encoding.UTF8));
		}
	}
}
