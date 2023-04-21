using DevilDaggersInfo.App.ContentSourceGen.Extensions;
using DevilDaggersInfo.App.ContentSourceGen.Utils;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace DevilDaggersInfo.App.ContentSourceGen.Generators.Content;

[Generator]
public class ContentGenerator : IIncrementalGenerator
{
	private const string _namespace = $"%{nameof(_namespace)}%";
	private const string _className = $"%{nameof(_className)}%";
	private const string _contentFieldInitializers = $"%{nameof(_contentFieldInitializers)}%";
	private const string _contentFields = $"%{nameof(_contentFields)}%";
	private const string _contentProperties = $"%{nameof(_contentProperties)}%";
	private const string _contentType = $"%{nameof(_contentType)}%";

	// TODO: Somehow prevent name collisions with content properties. Currently, a content property named "Initialize" or "InternalContentDictionary" would cause a name collision.
	// We could disallow underscores in the beginning of the property name, and rename "Initialize" and "InternalContentDictionary" to start with an underscore.
	// It would also be a good idea to emit diagnostics when incorrectly property names are used.
	private const string _template = $$"""
		using System;
		using System.Collections.Generic;
		using System.Linq;

		namespace {{_namespace}};

		public sealed class {{_className}} : DevilDaggersInfo.App.Engine.IContentContainer<{{_contentType}}>
		{
			{{_contentFields}}

			private static IReadOnlyDictionary<string, {{_contentType}}>? _internalContentDictionary;

			public static void Initialize(IReadOnlyDictionary<string, {{_contentType}}> content)
			{
				{{_contentFieldInitializers}}

				_internalContentDictionary = content;
			}

			{{_contentProperties}}

			public static IReadOnlyDictionary<string, {{_contentType}}> InternalContentDictionary => _internalContentDictionary ?? throw new InvalidOperationException("Content has not been initialized.");
		}
		""";

	private enum ContentType
	{
		Shader,
		Blob,
		Charset,
		Model,
		Sound,
		Texture,
	}

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		IncrementalValuesProvider<AdditionalText> contentFiles = context.AdditionalTextsProvider.Where(at => IsContentFile(Path.GetExtension(at.Path)));
		IncrementalValueProvider<(Compilation Compilation, ImmutableArray<AdditionalText> ContentFiles)> compilation = context.CompilationProvider.Combine(contentFiles.Collect());

		context.RegisterSourceOutput(
			compilation,
			static (spc, source) => Execute(source.Compilation, source.ContentFiles, spc));

		static void Execute(
			Compilation compilation,
			ImmutableArray<AdditionalText> contentFiles,
			SourceProductionContext context)
		{
			Dictionary<ContentType, List<string>> contentFileNamesByType = ((ContentType[])Enum.GetValues(typeof(ContentType))).ToDictionary(ct => ct, _ => new List<string>());
			foreach (string contentFilePath in contentFiles.Select(at => at.Path))
			{
				if (!PathIsValid(contentFilePath))
					continue;

				string contentFileName = Path.GetFileNameWithoutExtension(contentFilePath);
				string fileExtension = Path.GetExtension(contentFilePath);
				ContentType contentType = fileExtension switch
				{
					".vert" => ContentType.Shader,
					".geom" => ContentType.Shader,
					".frag" => ContentType.Shader,
					".bin" => ContentType.Blob,
					".txt" => ContentType.Charset,
					".obj" => ContentType.Model,
					".wav" => ContentType.Sound,
					".tga" => ContentType.Texture,
					_ => throw new InvalidOperationException($"Unknown content file extension: {fileExtension}"),
				};

				if (!contentFileNamesByType[contentType].Contains(contentFileName))
					contentFileNamesByType[contentType].Add(contentFileName);
			}

			foreach (KeyValuePair<ContentType, List<string>> kvp in contentFileNamesByType)
			{
				string className = kvp.Key switch
				{
					ContentType.Shader => "Shaders",
					ContentType.Blob => "Blobs",
					ContentType.Charset => "Charsets",
					ContentType.Model => "Models",
					ContentType.Sound => "Sounds",
					ContentType.Texture => "Textures",
					_ => throw new InvalidOperationException($"Unknown content type: {kvp.Key}"),
				};

				string contentTypeName = kvp.Key switch
				{
					ContentType.Shader => $"{Constants.RootNamespace}.Content.Shader",
					ContentType.Blob => $"{Constants.RootNamespace}.Content.Blob",
					ContentType.Charset => $"{Constants.RootNamespace}.Content.Charset",
					ContentType.Model => $"{Constants.RootNamespace}.Content.Model",
					ContentType.Sound => $"{Constants.RootNamespace}.Content.Sound",
					ContentType.Texture => $"{Constants.RootNamespace}.Content.Texture",
					_ => throw new InvalidOperationException($"Unknown content type: {kvp.Key}"),
				};

				CreateFile(context, compilation.AssemblyName, className, contentTypeName, kvp.Value);
			}
		}
	}

	private static bool IsContentFile(string fileExtension)
	{
		return fileExtension is ".vert" or ".geom" or ".frag" or ".bin" or ".txt" or ".obj" or ".wav" or ".tga";
	}

	/// <summary>
	/// Returns if the path is valid. The file name (without the extension) will be converted to a C# property, so it can only contain alphanumeric characters and cannot start with a digit.
	/// </summary>
	private static bool PathIsValid(string path)
	{
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
		if (fileNameWithoutExtension.Length == 0)
			return false;

		if (fileNameWithoutExtension.Any(c => !char.IsLetterOrDigit(c) && c != '_'))
			return false;

		if (fileNameWithoutExtension.Contains(',') || fileNameWithoutExtension.Contains('.'))
			return false;

		return !char.IsDigit(fileNameWithoutExtension[0]);
	}

	private static void CreateFile(SourceProductionContext context, string? gameNamespace, string className, string contentTypeName, List<string> contentFileNames)
	{
		string sourceBuilder = _template
			.Replace(_namespace, gameNamespace)
			.Replace(_className, className)
			.Replace(_contentType, contentTypeName)
			.Replace(_contentFieldInitializers, string.Join(Constants.NewLine, contentFileNames.ConvertAll(p => $"{GetFieldNameFromContentFileName(p)} = content.TryGetValue(\"{p}\", out {contentTypeName}? {GetLocalNameFromContentFileName(p)}) ? {GetLocalNameFromContentFileName(p)} : null;")).IndentCode(2))
			.Replace(_contentFields, string.Join(Constants.NewLine, contentFileNames.ConvertAll(p => $"private static {contentTypeName}? {GetFieldNameFromContentFileName(p)};")).IndentCode(1))
			.Replace(_contentProperties, string.Join(Constants.NewLine, contentFileNames.ConvertAll(p => $"public static {contentTypeName} {p} => {GetFieldNameFromContentFileName(p)} ?? throw new InvalidOperationException(\"Content does not exist or has not been initialized.\");")).IndentCode(1));

		context.AddSource(className, SourceBuilderUtils.Build(sourceBuilder));
	}

	private static string GetLocalNameFromContentFileName(string name)
	{
		return name.FirstCharToLowerCase();
	}

	private static string GetFieldNameFromContentFileName(string name)
	{
		return $"_{name.FirstCharToLowerCase()}";
	}
}
