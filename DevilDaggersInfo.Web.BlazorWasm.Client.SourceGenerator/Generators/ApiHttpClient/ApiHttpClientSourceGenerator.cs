using DevilDaggersInfo.Web.BlazorWasm.Client.SourceGenerator.Generators.ApiHttpClient.Enums;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.SourceGenerator.Generators.ApiHttpClient;

[Generator]
public class ApiHttpClientSourceGenerator : ISourceGenerator
{
	private const string _dtoUsings = $"%{nameof(_dtoUsings)}%";
	private const string _enumUsings = $"%{nameof(_enumUsings)}%";
	private const string _clientType = $"%{nameof(_clientType)}%";
	private const string _endpointMethods = $"%{nameof(_endpointMethods)}%";
	private const string _template = $@"{_dtoUsings}
{_enumUsings}
using DevilDaggersInfo.Web.BlazorWasm.Client.Utils;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;

public class {_clientType}ApiHttpClientGenerated
{{
	private readonly HttpClient _client;

	public {_clientType}ApiHttpClientGenerated(HttpClient client)
	{{
		_client = client;
	}}

{_endpointMethods}
}}
";

	private const string _returnType = $"%{nameof(_returnType)}%";
	private const string _methodName = $"%{nameof(_methodName)}%";
	private const string _methodParameters = $"%{nameof(_methodParameters)}%";
	private const string _queryParameters = $"%{nameof(_queryParameters)}%";
	private const string _apiRoute = $"%{nameof(_apiRoute)}%";
	private const string _endpointTemplate = $@"public async Task<{_returnType}> {_methodName}({_methodParameters})
{{
	Dictionary<string, object?> queryParameters = new()
	{{
{_queryParameters}
	}};
	return await _client.GetFromJsonAsync<{_returnType}>(UrlBuilderUtils.BuildUrlWithQuery(""{_apiRoute}"", queryParameters)) ?? throw new JsonDeserializationException();
}}
";

	private readonly ApiHttpClientContext _apiHttpClientContext = new();

	public void Initialize(GeneratorInitializationContext context)
	{
		_apiHttpClientContext.FindUsings();
		_apiHttpClientContext.FindEndpoints();
	}

	public void Execute(GeneratorExecutionContext context)
	{
		foreach (ClientType clientType in Constants.ClientTypes)
			GenerateClient(context, clientType, _apiHttpClientContext.Endpoints[clientType]);
	}

	private void GenerateClient(GeneratorExecutionContext context, ClientType clientType, List<Endpoint> endpoints)
	{
		List<string> endpointMethods = new();
		foreach (Endpoint endpoint in endpoints)
		{
			string methodParameters = string.Join(", ", endpoint.RouteParameters.Concat(endpoint.QueryParameters).Select(p => $"{p.Type} {p.Name}").ToList());
			string queryParameters = string.Join($",{Environment.NewLine}", endpoint.QueryParameters.ConvertAll(p => $"{{nameof({p.Name}), {p.Name}}}"));

			endpointMethods.Add(_endpointTemplate
				.Replace(_returnType, endpoint.ReturnType)
				.Replace(_methodName, endpoint.MethodName)
				.Replace(_methodParameters, methodParameters)
				.Replace(_queryParameters, queryParameters.Indent(2))
				.Replace(_apiRoute, endpoint.ApiRoute));
		}

		string code = _template
			.Replace(_dtoUsings, string.Join(Environment.NewLine, _apiHttpClientContext.GlobalUsings[IncludedDirectory.Dto].Concat(_apiHttpClientContext.SpecificUsings[IncludedDirectory.Dto][clientType]).Select(s => s.ToUsingDirective())))
			.Replace(_enumUsings, string.Join(Environment.NewLine, _apiHttpClientContext.GlobalUsings[IncludedDirectory.Enums].Concat(_apiHttpClientContext.SpecificUsings[IncludedDirectory.Enums][clientType]).Select(s => s.ToUsingDirective())))
			.Replace(_clientType, clientType.ToString())
			.Replace(_endpointMethods, string.Join(Environment.NewLine, endpointMethods).Indent(1));
		context.AddSource($"{clientType}ApiHttpClientGenerated", code);
	}
}
