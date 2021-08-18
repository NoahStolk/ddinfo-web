namespace DevilDaggersInfo.Core.Wiki.SourceGenerator.Generators;

[Generator]
public class EnemySourceGenerator : ISourceGenerator
{
	private const string _className = $"%{nameof(_className)}%";
	private const string _enemyFields = $"%{nameof(_enemyFields)}%";
	private const string _template = $@"namespace DevilDaggersInfo.Core.Wiki;

public static class {_className}
{{
{_enemyFields}

	public static readonly List<Enemy> All = typeof({_className}).GetFields().Where(f => f.FieldType == typeof(Enemy)).Select(f => (Enemy)f.GetValue(null)!).ToList();
}}
";

	public void Initialize(GeneratorInitializationContext context)
	{
		// Method intentionally left empty.
	}

	public void Execute(GeneratorExecutionContext context)
	{
		foreach (AdditionalText additionalText in context.AdditionalFiles.Where(at => Path.GetFileNameWithoutExtension(at.Path).StartsWith("Enemies")))
		{
			string gameVersion = Path.GetFileNameWithoutExtension(additionalText.Path).Replace("Enemies", string.Empty);
			string className = $"Enemies{gameVersion}";

			string? fileContents = additionalText.GetText()?.ToString();
			if (fileContents == null)
				continue;

			string[] lines = fileContents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			string[] fieldLines = new string[lines.Length];

			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];

				string[] parameters = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
				const int minimumParameterCount = 10;
				if (parameters.Length < minimumParameterCount)
					throw new($"Invalid specification in line '{line}'. There should be at least {minimumParameterCount} parameters, but only {parameters.Length} were found.");

				string name = parameters[0];
				string displayName = parameters[1];
				string colorProperty = parameters[2];
				int hp = int.Parse(parameters[3]);
				int gems = int.Parse(parameters[4]);
				int noFarmGems = int.Parse(parameters[5]);
				string death = parameters[6];
				float? homing3 = parameters[7] == "!" ? null : float.Parse(parameters[7]);
				float? homing4 = parameters[8] == "!" ? null : float.Parse(parameters[8]);
				int? spawn = parameters[9] == "!" ? null : int.Parse(parameters[9]);

				string[] remainingParameters = parameters.Skip(10).ToArray();
				string? enemies = null;
				if (remainingParameters.Length > 0)
					enemies = $", {string.Join(", ", remainingParameters)}";

				fieldLines[i] = $"\tpublic static readonly Enemy {name} = new(GameVersion.{gameVersion}, \"{displayName}\", EnemyColors.{colorProperty}, {hp}, {gems}, {noFarmGems}, Deaths{gameVersion}.{death}, new({ValueOrNull(homing3)}, {ValueOrNull(homing4)}), {ValueOrNull(spawn)}{enemies});";
			}

			string sourceBuilder = _template
				.Replace(_className, className)
				.Replace(_enemyFields, string.Join(Environment.NewLine, fieldLines));

			string warningSuppressionCodes = string.Join(", ", new[] { "CS0105", "CS1591", "CS8618", "S1128", "SA1001", "SA1027", "SA1028", "SA1101", "SA1122", "SA1137", "SA1200", "SA1201", "SA1208", "SA1210", "SA1309", "SA1311", "SA1413", "SA1503", "SA1505", "SA1507", "SA1508", "SA1516", "SA1600", "SA1601", "SA1602", "SA1623", "SA1649" });
			string warningsDisable = $"#pragma warning disable {warningSuppressionCodes}\n";
			string warningsRestore = $"\n#pragma warning restore {warningSuppressionCodes}";
			context.AddSource(className, SourceText.From(warningsDisable + sourceBuilder + warningsRestore, Encoding.UTF8));
		}
	}

	private static string ValueOrNull(float? value)
		=> value.HasValue ? $"{value.Value}f" : "null";

	private static string ValueOrNull(int? value)
		=> value.HasValue ? value.Value.ToString() : "null";
}
