using DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient.Endpoints;
using DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient.Enums;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient;

[Generator]
public class PublicApiHttpClientSourceGenerator : ISourceGenerator
{
	private const string _usings = $"%{nameof(_usings)}%";
	private const string _endpointMethods = $"%{nameof(_endpointMethods)}%";
	private const string _template = $@"#pragma warning disable CS1591
{_usings}
using DevilDaggersInfo.Web.BlazorWasm.Client.Utils;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;

public partial class PublicApiHttpClient
{{
{_endpointMethods}
}}
";

	private readonly ApiHttpClientContext _apiHttpClientContext = new();

	public void Initialize(GeneratorInitializationContext context)
	{
		_apiHttpClientContext.Clear();
		_apiHttpClientContext.AddUsings(ClientType.Public, IncludedDirectory.Dto);
		_apiHttpClientContext.AddUsings(ClientType.Public, IncludedDirectory.Enums);
		_apiHttpClientContext.AddEndpoints(ClientType.Public);
	}

	public void Execute(GeneratorExecutionContext context)
	{
		List<string> endpointMethods = new();
		foreach (Endpoint endpoint in _apiHttpClientContext.Endpoints)
			endpointMethods.Add(endpoint.Build());

		string code = _template
			.Replace(_usings, string.Join(Environment.NewLine, _apiHttpClientContext.Usings.Select(s => s.ToUsingDirective())))
			.Replace(_endpointMethods, string.Join(Environment.NewLine, endpointMethods).Indent(1));
		context.AddSource("PublicApiHttpClientGenerated", SourceText.From(SourceBuilderUtils.WrapInsideWarningSuppressionDirectives(code), Encoding.UTF8));
	}
}
