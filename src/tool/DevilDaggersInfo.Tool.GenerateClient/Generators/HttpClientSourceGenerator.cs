using DevilDaggersInfo.CommonSourceGen;
using DevilDaggersInfo.Tool.GenerateClient.Generators.Endpoints;

namespace DevilDaggersInfo.Tool.GenerateClient.Generators;

public class HttpClientSourceGenerator
{
	private const string _usings = $"%{nameof(_usings)}%";
	private const string _className = $"%{nameof(_className)}%";
	private const string _endpointMethods = $"%{nameof(_endpointMethods)}%";
	private const string _template = $@"{_usings}

namespace DevilDaggersInfo.Web.Client.HttpClients;

public partial class {_className}
{{
{_endpointMethods}
}}
";

	private readonly string _controllersSubDirectory;
	private readonly string _partialClassName;
	private readonly string _outputPath;

	public HttpClientSourceGenerator(string controllersSubDirectory, string partialClassName, string outputPath)
	{
		_controllersSubDirectory = controllersSubDirectory;
		_partialClassName = partialClassName;
		_outputPath = outputPath;
	}

	public void Execute()
	{
		ApiHttpClientContext apiHttpClientContext = new();
		apiHttpClientContext.AddUsings("DevilDaggersInfo.Web.Client.Utils", "System.Net.Http.Json");
		apiHttpClientContext.AddEndpoints(_controllersSubDirectory);

		List<string> endpointMethods = new();
		foreach (Endpoint endpoint in apiHttpClientContext.Endpoints)
			endpointMethods.Add(endpoint.Build());

		string code = _template
			.Replace(_usings, string.Join(Environment.NewLine, apiHttpClientContext.GetOrderedUsingDirectives()))
			.Replace(_className, _partialClassName)
			.Replace(_endpointMethods, string.Join(Environment.NewLine, endpointMethods).IndentCode(1));
		File.WriteAllText(_outputPath, code.WrapCodeInsideWarningSuppressionDirectives().TrimCode());
	}
}
