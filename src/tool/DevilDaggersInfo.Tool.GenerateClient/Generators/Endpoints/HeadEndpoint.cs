using DevilDaggersInfo.CommonSourceGen;

namespace DevilDaggersInfo.Tool.GenerateClient.Generators.Endpoints;

internal class HeadEndpoint : Endpoint
{
	private const string _methodName = $"%{nameof(_methodName)}%";
	private const string _methodParameters = $"%{nameof(_methodParameters)}%";
	private const string _queryParameters = $"%{nameof(_queryParameters)}%";
	private const string _apiRoute = $"%{nameof(_apiRoute)}%";
	private const string _httpMethod = $"%{nameof(_httpMethod)}%";
	private const string _endpointTemplate = $@"public async Task<HttpResponseMessage> {_methodName}({_methodParameters})
{{
	return await SendRequest(new HttpMethod(""{_httpMethod}""), $""{_apiRoute}"");
}}
";
	private const string _endpointWithQueryTemplate = $@"public async Task<HttpResponseMessage> {_methodName}({_methodParameters})
{{
	Dictionary<string, object?> queryParameters = new()
	{{
{_queryParameters}
	}};
	return await SendRequest(new HttpMethod(""{_httpMethod}""), BuildUrlWithQuery($""{_apiRoute}"", queryParameters));
}}
";

	public HeadEndpoint(string methodName, string apiRoute, Parameter? routeParameter, List<Parameter> queryParameters)
		: base(HttpMethod.Head, methodName, apiRoute)
	{
		QueryParameters = queryParameters;
		RouteParameter = routeParameter;
	}

	public Parameter? RouteParameter { get; }
	public List<Parameter> QueryParameters { get; }

	public override string Build()
	{
		List<Parameter> allParameters = new();
		if (RouteParameter != null)
			allParameters.Add(RouteParameter);

		allParameters.AddRange(QueryParameters);
		string methodParameters = string.Join(", ", allParameters.ConvertAll(p => p.ToString()));

		if (QueryParameters.Count == 0)
		{
			return _endpointTemplate
				.Replace(_methodName, MethodName)
				.Replace(_methodParameters, methodParameters)
				.Replace(_httpMethod, HttpMethod.ToString())
				.Replace(_apiRoute, ApiRoute);
		}

		string queryParameters = string.Join($",{Environment.NewLine}", QueryParameters.ConvertAll(p => p.Build()));
		return _endpointWithQueryTemplate
			.Replace(_methodName, MethodName)
			.Replace(_methodParameters, methodParameters)
			.Replace(_queryParameters, queryParameters.IndentCode(2))
			.Replace(_httpMethod, HttpMethod.ToString())
			.Replace(_apiRoute, ApiRoute);
	}

	public override string ToString()
		=> $"{HttpMethod} {ApiRoute} {MethodName}({string.Join(", ", RouteParameter)} | {string.Join(", ", QueryParameters)})";
}
