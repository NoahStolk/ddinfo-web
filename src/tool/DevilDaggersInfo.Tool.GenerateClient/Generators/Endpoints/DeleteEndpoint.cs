namespace DevilDaggersInfo.Tool.GenerateClient.Generators.Endpoints;

internal class DeleteEndpoint : Endpoint
{
	private const string _methodNameTemplate = $"%{nameof(_methodNameTemplate)}%";
	private const string _routeParameterTemplate = $"%{nameof(_routeParameterTemplate)}%";
	private const string _apiRouteTemplate = $"%{nameof(_apiRouteTemplate)}%";
	private const string _httpMethodTemplate = $"%{nameof(_httpMethodTemplate)}%";
	private const string _endpointTemplate = $$"""
		public async Task<HttpResponseMessage> {{_methodNameTemplate}}({{_routeParameterTemplate}})
		{
			return await SendRequest(new HttpMethod("{{_httpMethodTemplate}}"), $"{{_apiRouteTemplate}}");
		}

		""";

	private readonly Parameter _routeParameter;

	public DeleteEndpoint(string methodName, string apiRoute, Parameter routeParameter)
		: base(HttpMethod.Delete, methodName, apiRoute)
	{
		_routeParameter = routeParameter;
	}

	public override string Build()
	{
		return _endpointTemplate
			.Replace(_methodNameTemplate, MethodName)
			.Replace(_routeParameterTemplate, _routeParameter.ToString())
			.Replace(_httpMethodTemplate, HttpMethod.ToString())
			.Replace(_apiRouteTemplate, ApiRoute);
	}
}
