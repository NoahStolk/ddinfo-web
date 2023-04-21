using DevilDaggersInfo.App.ContentSourceGen.Extensions;
using DevilDaggersInfo.App.ContentSourceGen.Utils;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace DevilDaggersInfo.App.ContentSourceGen.Generators.ShaderUniform;

[Generator]
public class ShaderUniformGenerator : IIncrementalGenerator
{
	private const string _namespacePlaceholder = "%namespace%";
	private const string _classNamePlaceholder = "%className%";
	private const string _uniformPropertiesPlaceholder = "%uniformProperties%";
	private const string _uniformInitializationPlaceholder = "%uniformInitialization%";

	private const string _uniformsTemplate = $$"""
		namespace {{_namespacePlaceholder}};

		public static class {{_classNamePlaceholder}}
		{
			{{_uniformPropertiesPlaceholder}}

			public static void Initialize()
			{
				{{_uniformInitializationPlaceholder}}
			}
		}
		""";

	private const string _uniformCollectionInitializationPlaceholder = "%uniformCollectionInitialization%";

	private const string _initializerTemplate = $$"""
		using {{Constants.RootNamespace}};

		namespace {{_namespacePlaceholder}};

		public class ShaderUniformInitializer : IShaderUniformInitializer
		{
			public static void Initialize()
			{
				{{_uniformCollectionInitializationPlaceholder}}
			}
		}
		""";

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		IncrementalValuesProvider<AdditionalText> shaderFiles = context.AdditionalTextsProvider.Where(at => Path.GetExtension(at.Path) is ".vert" or ".geom" or ".frag");

		IncrementalValueProvider<(Compilation Compilation, ImmutableArray<AdditionalText> ShaderFiles)> compilation = context.CompilationProvider.Combine(shaderFiles.Collect());

		context.RegisterSourceOutput(
			compilation,
			static (spc, source) => Execute(source.Compilation, source.ShaderFiles, spc));

		static void Execute(
			Compilation compilation,
			ImmutableArray<AdditionalText> shaderFiles,
			SourceProductionContext context)
		{
			List<AdditionalText> vertexShaders = shaderFiles.Where(sf => Path.GetExtension(sf.Path) == ".vert").ToList();
			List<AdditionalText> geometryShaders = shaderFiles.Where(sf => Path.GetExtension(sf.Path) == ".geom").ToList();
			List<AdditionalText> fragmentShaders = shaderFiles.Where(sf => Path.GetExtension(sf.Path) == ".frag").ToList();

			foreach (AdditionalText vertexShader in vertexShaders)
			{
				string shaderName = Path.GetFileNameWithoutExtension(vertexShader.Path);

				AdditionalText? geometryShader = geometryShaders.Find(gs => Path.GetFileNameWithoutExtension(gs.Path) == shaderName);
				AdditionalText? fragmentShader = fragmentShaders.Find(fs => Path.GetFileNameWithoutExtension(fs.Path) == shaderName);

				List<ShaderUniform> uniformNames = GetUniformsFromGlslFile(vertexShader);
				if (geometryShader != null)
					uniformNames.AddRange(GetUniformsFromGlslFile(geometryShader));
				if (fragmentShader != null)
					uniformNames.AddRange(GetUniformsFromGlslFile(fragmentShader));

				uniformNames = uniformNames.Distinct().ToList();

				// Generate the class, even if there are no uniforms.
				string className = $"{shaderName}Uniforms";
				string uniformsSourceBuilder = _uniformsTemplate
					.Replace(_namespacePlaceholder, compilation.AssemblyName)
					.Replace(_classNamePlaceholder, className)
					.Replace(_uniformPropertiesPlaceholder, string.Join(Constants.NewLine, uniformNames.ConvertAll(u => $"public static int {u.PropertyName} {{ get; private set; }}")).IndentCode(1))
					.Replace(_uniformInitializationPlaceholder, string.Join(Constants.NewLine, uniformNames.ConvertAll(u => $"{u.PropertyName} = {Constants.RootNamespace}.Content.Shader.GetUniformLocation(Shaders.{shaderName}.Id, \"{u.Name}\");")).IndentCode(2));

				context.AddSource(className, SourceBuilderUtils.Build(uniformsSourceBuilder));
			}

			List<string> shaderNames = shaderFiles.Select(vs => Path.GetFileNameWithoutExtension(vs.Path)).ToList();

			// This file must always be generated.
			string initializerSourceBuilder = _initializerTemplate
				.Replace(_namespacePlaceholder, compilation.AssemblyName)
				.Replace(_uniformCollectionInitializationPlaceholder, string.Join(Constants.NewLine, shaderNames.ConvertAll(u => $"{u}Uniforms.Initialize();")).IndentCode(2));

			context.AddSource("ShaderUniformInitializer", SourceBuilderUtils.Build(initializerSourceBuilder));
		}
	}

	private static List<ShaderUniform> GetUniformsFromGlslFile(AdditionalText additionalText)
	{
		// ! LINQ query filters out null values.
		return additionalText.GetText()?.ToString().Split('\n').Select(GlslUtils.GetFromGlslLine).Where(su => su != null).ToList()!;
	}
}
