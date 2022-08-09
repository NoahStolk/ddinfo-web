namespace DevilDaggersInfo.Core.Asset.SourceGen;

[Generator]
public class AssetDataSourceGenerator : IIncrementalGenerator
{
	private const string _className = $"%{nameof(_className)}%";
	private const string _assetTypeName = $"%{nameof(_assetTypeName)}%";
	private const string _assetFields = $"%{nameof(_assetFields)}%";
	private const string _template = $@"namespace DevilDaggersInfo.Core.Asset;

public static class {_className}
{{
{_assetFields}

	public static readonly List<{_assetTypeName}> All = typeof({_className}).GetFields().Where(f => f.FieldType == typeof({_assetTypeName})).Select(f => ({_assetTypeName})f.GetValue(null)!).ToList();
}}
";

	private enum AssetType
	{
		Audio,
		Mesh,
		ObjectBinding,
		Shader,
		Texture,
	}

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterSourceOutput(context.AdditionalTextsProvider, static (spc, at) => Execute(spc, at));
	}

	private static void Execute(SourceProductionContext sourceProductionContext, AdditionalText additionalText)
	{
		string className = Path.GetFileNameWithoutExtension(additionalText.Path);
		string? fileContents = additionalText.GetText()?.ToString();
		if (fileContents == null)
			return;

		AssetType assetType = className.TrimStart("Audio", "Core", "Dd") switch
		{
			"Audio" => AssetType.Audio,
			"ObjectBindings" => AssetType.ObjectBinding,
			"Meshes" => AssetType.Mesh,
			"Shaders" => AssetType.Shader,
			"Textures" => AssetType.Texture,
			_ => throw new NotSupportedException(),
		};

		string assetTypeName = $"{assetType}AssetInfo";

		string[] lines = fileContents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
		string[] fieldLines = new string[lines.Length];

		for (int i = 0; i < lines.Length; i++)
		{
			string line = lines[i];

			string[] parameters = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
			const int minimumParameterCount = 2;
			if (parameters.Length < minimumParameterCount)
				throw new($"Invalid specification in line '{line}'. There should be at least {minimumParameterCount} parameters, but only {parameters.Length} were found.");

			string ctorParameters = assetType switch
			{
				AssetType.Audio => GetCtorParametersAudio(parameters[0], bool.Parse(parameters[1]), float.Parse(parameters[2]), bool.Parse(parameters[3])),
				AssetType.ObjectBinding => GetCtorParametersObjectBinding(parameters[0], bool.Parse(parameters[1])),
				AssetType.Mesh => GetCtorParametersMesh(parameters[0], bool.Parse(parameters[1]), int.Parse(parameters[2]), int.Parse(parameters[3])),
				AssetType.Shader => GetCtorParametersShader(parameters[0], bool.Parse(parameters[1])),
				AssetType.Texture => GetCtorParametersTexture(parameters[0], bool.Parse(parameters[1]), int.Parse(parameters[2]), int.Parse(parameters[3]), bool.Parse(parameters[4]), parameters.Length > 5 ? parameters[5] : null),
				_ => throw new NotSupportedException(),
			};

			fieldLines[i] = $"public static readonly {assetTypeName} {parameters[0]} = new({ctorParameters});";
		}

		string source = _template
			.Replace(_className, className)
			.Replace(_assetTypeName, assetTypeName)
			.Replace(_assetFields, string.Join(Environment.NewLine, fieldLines).IndentCode(1));

		sourceProductionContext.AddSource(className, SourceText.From(source.BuildSource(), Encoding.UTF8));

		static string GetCtorParametersAudio(string assetName, bool isProhibited, float defaultLoudness, bool presentInDefaultLoudness)
			=> $"{FormatString(assetName)}, {FormatBool(isProhibited)}, {FormatFloat(defaultLoudness)}, {FormatBool(presentInDefaultLoudness)}";

		static string GetCtorParametersObjectBinding(string assetName, bool isProhibited)
			=> $"{FormatString(assetName)}, {FormatBool(isProhibited)}";

		static string GetCtorParametersMesh(string assetName, bool isProhibited, int defaultIndexCount, int defaultVertexCount)
			=> $"{FormatString(assetName)}, {FormatBool(isProhibited)}, {defaultIndexCount}, {defaultVertexCount}";

		static string GetCtorParametersShader(string assetName, bool isProhibited)
			=> $"{FormatString(assetName)}, {FormatBool(isProhibited)}";

		static string GetCtorParametersTexture(string assetName, bool isProhibited, int defaultWidth, int defaultHeight, bool isTextureForMesh, string? objectBinding)
			=> $"{FormatString(assetName)}, {FormatBool(isProhibited)}, {defaultWidth}, {defaultHeight}, {isTextureForMesh.ToString().ToLower()}, {FormatString(objectBinding)}";

		static string FormatBool(bool value)
			=> value.ToString().ToLower();

		static string FormatFloat(float value)
			=> $"{value}f";

		static string FormatString(string? str)
			=> str == null ? "null" : $"\"{str}\"";
	}
}
