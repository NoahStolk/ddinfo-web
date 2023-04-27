using DevilDaggersInfo.Tool.GenerateClient.Extensions;

namespace DevilDaggersInfo.Tool.GenerateClient.Generators.Endpoints;

internal class HeadEndpoint : Endpoint
{
	private const string _methodNameTemplate = $"%{nameof(_methodNameTemplate)}%";
	private const string _methodParametersTemplate = $"%{nameof(_methodParametersTemplate)}%";
	private const string _queryParametersTemplate = $"%{nameof(_queryParametersTemplate)}%";
	private const string _apiRouteTemplate = $"%{nameof(_apiRouteTemplate)}%";
	private const string _httpMethodTemplate = $"%{nameof(_httpMethodTemplate)}%";
	private const string _endpointTemplate = $$"""
		public async Task<HttpResponseMessage> {{_methodNameTemplate}}({{_methodParametersTemplate}})
		{
			return await SendRequest(new HttpMethod("{{_httpMethodTemplate}}"), $"{{_apiRouteTemplate}}");
		}

		""";
	private const string _endpointWithQueryTemplate = $$"""
		public async Task<HttpResponseMessage> {{_methodNameTemplate}}({{_methodParametersTemplate}})
		{
			Dictionary<string, object?> queryParameters = new()
			{
		{{_queryParametersTemplate}}
			};
			return await SendRequest(new HttpMethod("{{_httpMethodTemplate}}"), BuildUrlWithQuery($"{{_apiRouteTemplate}}", queryParameters));
		}

		""";

	private readonly Parameter? _routeParameter;
	private readonly List<Parameter> _queryParameters;

	public HeadEndpoint(string methodName, string apiRoute, Parameter? routeParameter, List<Parameter> queryParameters)
		: base(HttpMethod.Head, methodName, apiRoute)
	{
		_queryParameters = queryParameters;
		_routeParameter = routeParameter;
	}

	public override string Build()
	{
		List<Parameter> allParameters = new();
		if (_routeParameter != null)
			allParameters.Add(_routeParameter);

		allParameters.AddRange(_queryParameters);
		string methodParameters = string.Join(", ", allParameters.ConvertAll(p => p.ToString()));

		if (_queryParameters.Count == 0)
		{
			return _endpointTemplate
				.Replace(_methodNameTemplate, MethodName)
				.Replace(_methodParametersTemplate, methodParameters)
				.Replace(_httpMethodTemplate, HttpMethod.ToString())
				.Replace(_apiRouteTemplate, ApiRoute);
		}

		string queryParameters = string.Join($",{Environment.NewLine}", _queryParameters.ConvertAll(p => p.BuildAsQueryParameter()));
		return _endpointWithQueryTemplate
			.Replace(_methodNameTemplate, MethodName)
			.Replace(_methodParametersTemplate, methodParameters)
			.Replace(_queryParametersTemplate, queryParameters.IndentCode(2))
			.Replace(_httpMethodTemplate, HttpMethod.ToString())
			.Replace(_apiRouteTemplate, ApiRoute);
	}

	public override string ToString()
		=> $"{HttpMethod} {ApiRoute} {MethodName}({string.Join(", ", _routeParameter)} | {string.Join(", ", _queryParameters)})";
}
