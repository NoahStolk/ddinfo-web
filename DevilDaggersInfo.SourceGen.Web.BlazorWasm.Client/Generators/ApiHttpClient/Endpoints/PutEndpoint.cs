namespace DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient.Endpoints;

internal class PutEndpoint : Endpoint
{
	private const string _methodName = $"%{nameof(_methodName)}%";
	private const string _routeParameter = $"%{nameof(_routeParameter)}%";
	private const string _bodyParameter = $"%{nameof(_bodyParameter)}%";
	private const string _apiRoute = $"%{nameof(_apiRoute)}%";
	private const string _httpMethod = $"%{nameof(_httpMethod)}%";
	private const string _endpointTemplate = $@"public async Task<HttpResponseMessage> {_methodName}({_routeParameter}, {_bodyParameter})
{{
	return await SendRequest(new HttpMethod(""{_httpMethod}""), $""{_apiRoute}"", {_bodyParameter});
}}
";

	public PutEndpoint(string methodName, string apiRoute, Parameter routeParameter, Parameter bodyParameter)
		: base(HttpMethod.Put, methodName, apiRoute)
	{
		RouteParameter = routeParameter;
		BodyParameter = bodyParameter;
	}

	public Parameter RouteParameter { get; }

	public Parameter BodyParameter { get; }

	public override string Build()
	{
		return _endpointTemplate
			.Replace(_methodName, MethodName)
			.Replace(_routeParameter, RouteParameter.ToString())
			.Replace(_bodyParameter, BodyParameter.ToString())
			.Replace(_httpMethod, HttpMethod.ToString())
			.Replace(_apiRoute, ApiRoute);
	}
}
