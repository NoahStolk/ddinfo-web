namespace DevilDaggersInfo.Tool.GenerateClient.Generators.Endpoints;

internal class PutEndpoint : Endpoint
{
	private const string _methodNameTemplate = $"%{nameof(_methodNameTemplate)}%";
	private const string _routeParameterTemplate = $"%{nameof(_routeParameterTemplate)}%";
	private const string _bodyParameterTypeTemplate = $"%{nameof(_bodyParameterTypeTemplate)}%";
	private const string _bodyParameterTemplate = $"%{nameof(_bodyParameterTemplate)}%";
	private const string _apiRouteTemplate = $"%{nameof(_apiRouteTemplate)}%";
	private const string _httpMethodTemplate = $"%{nameof(_httpMethodTemplate)}%";
	private const string _endpointTemplate = $$"""
		public async Task<HttpResponseMessage> {{_methodNameTemplate}}({{_routeParameterTemplate}}, {{_bodyParameterTypeTemplate}} {{_bodyParameterTemplate}})
		{
			return await SendRequest(new HttpMethod("{{_httpMethodTemplate}}"), $"{{_apiRouteTemplate}}", JsonContent.Create({{_bodyParameterTemplate}}));
		}

		""";

	private readonly Parameter _routeParameter;
	private readonly Parameter _bodyParameter;

	public PutEndpoint(string methodName, string apiRoute, Parameter routeParameter, Parameter bodyParameter)
		: base(HttpMethod.Put, methodName, apiRoute)
	{
		_routeParameter = routeParameter;
		_bodyParameter = bodyParameter;
	}

	public override string Build()
	{
		string bodyParameterStr = _bodyParameter.ToString();
		string bodyParameterType = bodyParameterStr[..bodyParameterStr.IndexOf(' ')];
		string bodyParameter = bodyParameterStr[(bodyParameterStr.IndexOf(' ') + 1)..];

		return _endpointTemplate
			.Replace(_methodNameTemplate, MethodName)
			.Replace(_routeParameterTemplate, _routeParameter.ToString())
			.Replace(_bodyParameterTypeTemplate, bodyParameterType)
			.Replace(_bodyParameterTemplate, bodyParameter)
			.Replace(_httpMethodTemplate, HttpMethod.ToString())
			.Replace(_apiRouteTemplate, ApiRoute);
	}
}
