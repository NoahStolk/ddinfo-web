using DevilDaggersInfo.CommonSourceGen;
using DevilDaggersInfo.Tool.GenerateClient.Generators.Endpoints;

namespace DevilDaggersInfo.Tool.GenerateClient.Generators;

public class HttpClientSourceGenerator
{
	// TODO: Use """ strings.
	private const string _usings = $"%{nameof(_usings)}%";
	private const string _namespace = $"%{nameof(_namespace)}%";
	private const string _className = $"%{nameof(_className)}%";
	private const string _endpointMethods = $"%{nameof(_endpointMethods)}%";
	private const string _template = $@"{_usings}

namespace {_namespace};

public partial class {_className}
{{
{_endpointMethods}
	private static string BuildUrlWithQuery(string baseUrl, Dictionary<string, object?> queryParameters)
	{{
		if (queryParameters.Count == 0)
			return baseUrl;

		string queryParameterString = string.Join('&', queryParameters.Select(kvp => $""{{kvp.Key}}={{kvp.Value}}""));
		return $""{{baseUrl.TrimEnd('/')}}?{{queryParameterString}}"";
	}}
}}
";

	private readonly string _controllersSubDirectory;
	private readonly string _namespaceForGeneratedClass;
	private readonly string _classNameForGeneratedClass;
	private readonly string _outputPath;

	public HttpClientSourceGenerator(string controllersSubDirectory, string namespaceForGeneratedClass, string classNameForGeneratedClass, string outputPath)
	{
		_controllersSubDirectory = controllersSubDirectory;
		_namespaceForGeneratedClass = namespaceForGeneratedClass;
		_classNameForGeneratedClass = classNameForGeneratedClass;
		_outputPath = outputPath;
	}

	public void Execute()
	{
		ApiHttpClientContext apiHttpClientContext = new();
		apiHttpClientContext.AddUsings("System.Net.Http.Json");
		apiHttpClientContext.AddEndpoints(_controllersSubDirectory);

		List<string> endpointMethods = new();
		foreach (Endpoint endpoint in apiHttpClientContext.Endpoints)
			endpointMethods.Add(endpoint.Build());

		string code = _template
			.Replace(_usings, string.Join(Environment.NewLine, apiHttpClientContext.GetOrderedUsingDirectives()))
			.Replace(_namespace, _namespaceForGeneratedClass)
			.Replace(_className, _classNameForGeneratedClass)
			.Replace(_endpointMethods, string.Join(Environment.NewLine, endpointMethods).IndentCode(1));
		File.WriteAllText(_outputPath, code.BuildSource());
	}
}
