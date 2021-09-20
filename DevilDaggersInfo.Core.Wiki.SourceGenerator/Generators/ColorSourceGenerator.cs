namespace DevilDaggersInfo.Core.Wiki.SourceGenerator.Generators;

[Generator]
public class ColorSourceGenerator : ISourceGenerator
{
	private const string _className = $"%{nameof(_className)}%";
	private const string _colorFields = $"%{nameof(_colorFields)}%";
	private const string _template = $@"namespace DevilDaggersInfo.Core.Wiki.Colors;

public static class {_className}
{{
{_colorFields}
}}
";

	public void Initialize(GeneratorInitializationContext context)
	{
		// Method intentionally left empty.
	}

	public void Execute(GeneratorExecutionContext context)
	{
		foreach (AdditionalText additionalText in context.AdditionalFiles.Where(at => at.Path.EndsWith("Colors.csv")))
		{
			string className = Path.GetFileNameWithoutExtension(additionalText.Path);
			string? fileContents = additionalText.GetText()?.ToString();
			if (fileContents == null)
				continue;

			string[] lines = fileContents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			string[] fieldLines = new string[lines.Length];

			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];

				string[] parameters = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
				const int parameterCount = 2;
				if (parameters.Length != parameterCount)
					throw new($"Invalid specification in line '{line}'. There should be {parameterCount} parameters, but {parameters.Length} were found.");

				// TODO: Check if it is hexadecimal.
				if (parameters[1].Length != 6)
					throw new($"Invalid color '{parameters[1]}'.");

				string objectName = parameters[0];
				string colorR = parameters[1].Substring(0, 2);
				string colorG = parameters[1].Substring(2, 2);
				string colorB = parameters[1].Substring(4, 2);

				fieldLines[i] = $"\tpublic static readonly Color {objectName} = new(0x{colorR}, 0x{colorG}, 0x{colorB});";
			}

			string source = _template
				.Replace(_className, className)
				.Replace(_colorFields, string.Join(Environment.NewLine, fieldLines));
			context.AddSource(className, SourceText.From(SourceBuilderUtils.WrapInsideWarningSuppressionDirectives(source), Encoding.UTF8));
		}
	}
}
