using DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient.Enums;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient;

[Generator]
public class AdminApiHttpClientSourceGenerator : ISourceGenerator
{
	private const string _usings = $"%{nameof(_usings)}%";
	private const string _endpointMethods = $"%{nameof(_endpointMethods)}%";
	private const string _template = $@"#pragma warning disable CS1591
{_usings}
using Blazored.LocalStorage;
using DevilDaggersInfo.Web.BlazorWasm.Client.Utils;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;

public class AdminApiHttpClient
{{
	private readonly ILocalStorageService _localStorageService;

	public AdminApiHttpClient(HttpClient client, ILocalStorageService localStorageService)
	{{
		Client = client;
		_localStorageService = localStorageService;
	}}

	// TODO: Make private.
	public HttpClient Client {{ get; }}

	private async Task<HttpResponseMessage> SendRequest(HttpMethod httpMethod, string url)
	{{
		HttpRequestMessage request = new()
		{{
			RequestUri = new Uri(url, UriKind.Relative),
			Method = httpMethod,
		}};

		string? token = await _localStorageService.GetItemAsStringAsync(""auth""); // TODO: Don't hardcode key string ""auth"".
		if (token != null)
			request.Headers.Authorization = new AuthenticationHeaderValue(""Bearer"", token);

		HttpResponseMessage httpResponseMessage = await Client.SendAsync(request);

		if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
			throw new UnauthorizedAccessException();

		if (!httpResponseMessage.IsSuccessStatusCode)
			throw new();

		return httpResponseMessage;
	}}

	private async Task<T> SendGetRequest<T>(string url)
	{{
		HttpResponseMessage response = await SendRequest(HttpMethod.Get, url);
		return await response.Content.ReadFromJsonAsync<T>() ?? throw new JsonDeserializationException();
	}}

{_endpointMethods}
}}
";

	private const string _returnType = $"%{nameof(_returnType)}%";
	private const string _methodName = $"%{nameof(_methodName)}%";
	private const string _methodParameters = $"%{nameof(_methodParameters)}%";
	private const string _queryParameters = $"%{nameof(_queryParameters)}%";
	private const string _apiRoute = $"%{nameof(_apiRoute)}%";
	private const string _httpMethod = $"%{nameof(_httpMethod)}%";
	private const string _getEndpointTemplate = $@"public async Task<{_returnType}> {_methodName}({_methodParameters})
{{
	return await SendGetRequest<{_returnType}>($""{_apiRoute}"");
}}
";
	private const string _getEndpointWithQueryTemplate = $@"public async Task<{_returnType}> {_methodName}({_methodParameters})
{{
	Dictionary<string, object?> queryParameters = new()
	{{
{_queryParameters}
	}};
	return await SendGetRequest<{_returnType}>(UrlBuilderUtils.BuildUrlWithQuery($""{_apiRoute}"", queryParameters));
}}
";
	private const string _endpointTemplate = $@"public async Task<HttpResponseMessage> {_methodName}({_methodParameters})
{{
	return await SendRequest(new HttpMethod(""{_httpMethod}""), $""{_apiRoute}"");
}}
";

	private readonly ApiHttpClientContext _apiHttpClientContext = new();

	public void Initialize(GeneratorInitializationContext context)
	{
		_apiHttpClientContext.Clear();
		_apiHttpClientContext.AddUsings(ClientType.Admin, IncludedDirectory.Dto);
		_apiHttpClientContext.AddUsings(ClientType.Admin, IncludedDirectory.Enums);
		_apiHttpClientContext.AddEndpoints(ClientType.Admin);
	}

	public void Execute(GeneratorExecutionContext context)
	{
		List<string> endpointMethods = new();
		foreach (Endpoint endpoint in _apiHttpClientContext.Endpoints)
		{
			string methodParameters = string.Join(", ", endpoint.RouteParameters.Concat(endpoint.QueryParameters).Select(p => $"{p.Type} {p.Name}").ToList());
			string queryParameters = string.Join($",{Environment.NewLine}", endpoint.QueryParameters.ConvertAll(p => $"{{nameof({p.Name}), {p.Name}}}"));

			if (endpoint.HttpMethod == HttpMethod.Get)
			{
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
			else
			{
				endpointMethods.Add(_endpointTemplate
					.Replace(_methodName, endpoint.MethodName)
					.Replace(_methodParameters, methodParameters)
					.Replace(_httpMethod, endpoint.HttpMethod.ToString())
					.Replace(_apiRoute, endpoint.ApiRoute));
			}
		}

		string code = _template
			.Replace(_usings, string.Join(Environment.NewLine, _apiHttpClientContext.Usings.Select(s => s.ToUsingDirective())))
			.Replace(_endpointMethods, string.Join(Environment.NewLine, endpointMethods).Indent(1));
		context.AddSource("AdminApiHttpClientGenerated", SourceText.From(SourceBuilderUtils.WrapInsideWarningSuppressionDirectives(code), Encoding.UTF8));
	}
}
