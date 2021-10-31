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

public class PublicApiHttpClient
{{
	public PublicApiHttpClient(HttpClient client)
	{{
		Client = client;
	}}

	public HttpClient Client {{ get; }}

{_endpointMethods}
}}
";

	private const string _returnType = $"%{nameof(_returnType)}%";
	private const string _methodName = $"%{nameof(_methodName)}%";
	private const string _methodParameters = $"%{nameof(_methodParameters)}%";
	private const string _queryParameters = $"%{nameof(_queryParameters)}%";
	private const string _apiRoute = $"%{nameof(_apiRoute)}%";
	private const string _getEndpointTemplate = $@"public async Task<{_returnType}> {_methodName}({_methodParameters})
{{
	return await Client.GetFromJsonAsync<{_returnType}>($""{_apiRoute}"") ?? throw new JsonDeserializationException();
}}
";
	private const string _getEndpointWithQueryTemplate = $@"public async Task<{_returnType}> {_methodName}({_methodParameters})
{{
	Dictionary<string, object?> queryParameters = new()
	{{
{_queryParameters}
	}};
	return await Client.GetFromJsonAsync<{_returnType}>(UrlBuilderUtils.BuildUrlWithQuery($""{_apiRoute}"", queryParameters)) ?? throw new JsonDeserializationException();
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
		{
			string methodParameters = string.Join(", ", endpoint.RouteParameters.Concat(endpoint.QueryParameters).Select(p => $"{p.Type} {p.Name}").ToList());
			string queryParameters = string.Join($",{Environment.NewLine}", endpoint.QueryParameters.ConvertAll(p => $"{{nameof({p.Name}), {p.Name}}}"));

			if (endpoint.QueryParameters.Count == 0)
			{
				endpointMethods.Add(_getEndpointTemplate
					.Replace(_returnType, endpoint.ReturnType)
					.Replace(_methodName, endpoint.MethodName)
					.Replace(_methodParameters, methodParameters)
					.Replace(_apiRoute, endpoint.ApiRoute));
			}
			else
			{
				endpointMethods.Add(_getEndpointWithQueryTemplate
					.Replace(_returnType, endpoint.ReturnType)
					.Replace(_methodName, endpoint.MethodName)
					.Replace(_methodParameters, methodParameters)
					.Replace(_queryParameters, queryParameters.Indent(2))
					.Replace(_apiRoute, endpoint.ApiRoute));
			}
		}

		string code = _template
			.Replace(_usings, string.Join(Environment.NewLine, _apiHttpClientContext.Usings.Select(s => s.ToUsingDirective())))
			.Replace(_endpointMethods, string.Join(Environment.NewLine, endpointMethods).Indent(1));
		context.AddSource("PublicApiHttpClientGenerated", SourceText.From(SourceBuilderUtils.WrapInsideWarningSuppressionDirectives(code), Encoding.UTF8));
	}
}
