using DevilDaggersInfo.DevUtil.GenerateClient.Extensions;

namespace DevilDaggersInfo.DevUtil.GenerateClient.Generators.Endpoints;

internal class GetEndpoint : Endpoint
{
	private const string _returnTypeTemplate = $"%{nameof(_returnTypeTemplate)}%";
	private const string _methodNameTemplate = $"%{nameof(_methodNameTemplate)}%";
	private const string _methodParametersTemplate = $"%{nameof(_methodParametersTemplate)}%";
	private const string _queryParametersTemplate = $"%{nameof(_queryParametersTemplate)}%";
	private const string _apiRouteTemplate = $"%{nameof(_apiRouteTemplate)}%";
	private const string _endpointTemplate = $$"""
		public async Task<{{_returnTypeTemplate}}> {{_methodNameTemplate}}({{_methodParametersTemplate}})
		{
			return await SendGetRequest<{{_returnTypeTemplate}}>($"{{_apiRouteTemplate}}");
		}

		""";
	private const string _endpointWithQueryTemplate = $$"""
		public async Task<{{_returnTypeTemplate}}> {{_methodNameTemplate}}({{_methodParametersTemplate}})
		{
			Dictionary<string, object?> queryParameters = new()
			{
		{{_queryParametersTemplate}}
			};
			return await SendGetRequest<{{_returnTypeTemplate}}>(BuildUrlWithQuery($"{{_apiRouteTemplate}}", queryParameters));
		}

		""";

	private readonly Parameter? _routeParameter;
	private readonly List<Parameter> _queryParameters;
	private readonly string _returnType;

	public GetEndpoint(string methodName, string apiRoute, Parameter? routeParameter, List<Parameter> queryParameters, string returnType)
		: base(HttpMethod.Get, methodName, apiRoute)
	{
		_queryParameters = queryParameters;
		_routeParameter = routeParameter;
		_returnType = returnType;
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
				.Replace(_returnTypeTemplate, _returnType)
				.Replace(_methodNameTemplate, MethodName)
				.Replace(_methodParametersTemplate, methodParameters)
				.Replace(_apiRouteTemplate, ApiRoute);
		}

		string queryParameters = string.Join($",{Environment.NewLine}", _queryParameters.ConvertAll(p => p.BuildAsQueryParameter()));
		return _endpointWithQueryTemplate
			.Replace(_returnTypeTemplate, _returnType)
			.Replace(_methodNameTemplate, MethodName)
			.Replace(_methodParametersTemplate, methodParameters)
			.Replace(_queryParametersTemplate, queryParameters.IndentCode(2))
			.Replace(_apiRouteTemplate, ApiRoute);
	}

	public override string ToString()
		=> $"{HttpMethod} {ApiRoute} {_returnType} {MethodName}({string.Join(", ", _routeParameter)} | {string.Join(", ", _queryParameters)})";
}
