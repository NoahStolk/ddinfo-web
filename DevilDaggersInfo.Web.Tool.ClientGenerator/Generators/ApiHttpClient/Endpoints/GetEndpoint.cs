namespace DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient.Endpoints;

internal class GetEndpoint : Endpoint
{
	private const string _returnType = $"%{nameof(_returnType)}%";
	private const string _methodName = $"%{nameof(_methodName)}%";
	private const string _methodParameters = $"%{nameof(_methodParameters)}%";
	private const string _queryParameters = $"%{nameof(_queryParameters)}%";
	private const string _apiRoute = $"%{nameof(_apiRoute)}%";
	private const string _endpointTemplate = $@"public async Task<{_returnType}> {_methodName}({_methodParameters})
{{
	return await SendGetRequest<{_returnType}>($""{_apiRoute}"");
}}
";
	private const string _endpointWithQueryTemplate = $@"public async Task<{_returnType}> {_methodName}({_methodParameters})
{{
	Dictionary<string, object?> queryParameters = new()
	{{
{_queryParameters}
	}};
	return await SendGetRequest<{_returnType}>(UrlBuilderUtils.BuildUrlWithQuery($""{_apiRoute}"", queryParameters));
}}
";

	public GetEndpoint(string methodName, string apiRoute, Parameter? routeParameter, List<Parameter> queryParameters, string returnType)
		: base(HttpMethod.Get, methodName, apiRoute)
	{
		QueryParameters = queryParameters;
		RouteParameter = routeParameter;
		ReturnType = returnType;
	}

	public Parameter? RouteParameter { get; }
	public List<Parameter> QueryParameters { get; }
	public string ReturnType { get; }

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
				.Replace(_returnType, ReturnType)
				.Replace(_methodName, MethodName)
				.Replace(_methodParameters, methodParameters)
				.Replace(_apiRoute, ApiRoute);
		}

		string queryParameters = string.Join($",{Environment.NewLine}", QueryParameters.ConvertAll(p => $"{{ nameof({p.Name}), {p.Name} }}"));
		return _endpointWithQueryTemplate
			.Replace(_returnType, ReturnType)
			.Replace(_methodName, MethodName)
			.Replace(_methodParameters, methodParameters)
			.Replace(_queryParameters, queryParameters.IndentCode(2))
			.Replace(_apiRoute, ApiRoute);
	}

	public override string ToString()
		=> $"{HttpMethod} {ApiRoute} {ReturnType} {MethodName}({string.Join(", ", RouteParameter)} | {string.Join(", ", QueryParameters)})";
}
