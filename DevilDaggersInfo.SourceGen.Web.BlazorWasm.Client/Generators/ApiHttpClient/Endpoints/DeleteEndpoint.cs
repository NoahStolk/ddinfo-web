namespace DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient.Endpoints;

internal class DeleteEndpoint : Endpoint
{
	private const string _methodName = $"%{nameof(_methodName)}%";
	private const string _routeParameter = $"%{nameof(_routeParameter)}%";
	private const string _apiRoute = $"%{nameof(_apiRoute)}%";
	private const string _httpMethod = $"%{nameof(_httpMethod)}%";
	private const string _endpointTemplate = $@"public async Task<HttpResponseMessage> {_methodName}({_routeParameter})
{{
	return await SendRequest(new HttpMethod(""{_httpMethod}""), $""{_apiRoute}"");
}}
";

	public DeleteEndpoint(string methodName, string apiRoute, Parameter routeParameter)
		: base(HttpMethod.Delete, methodName, apiRoute)
	{
		RouteParameter = routeParameter;
	}

	public Parameter RouteParameter { get; }

	public override string Build()
	{
		return _endpointTemplate
			.Replace(_methodName, MethodName)
			.Replace(_routeParameter, RouteParameter.ToString())
			.Replace(_httpMethod, HttpMethod.ToString())
			.Replace(_apiRoute, ApiRoute);
	}
}
