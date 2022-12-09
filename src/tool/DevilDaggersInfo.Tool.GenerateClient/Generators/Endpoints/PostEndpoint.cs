namespace DevilDaggersInfo.Tool.GenerateClient.Generators.Endpoints;

internal class PostEndpoint : Endpoint
{
	private const string _methodNameTemplate = $"%{nameof(_methodNameTemplate)}%";
	private const string _bodyParameterTypeTemplate = $"%{nameof(_bodyParameterTypeTemplate)}%";
	private const string _bodyParameterTemplate = $"%{nameof(_bodyParameterTemplate)}%";
	private const string _apiRouteTemplate = $"%{nameof(_apiRouteTemplate)}%";
	private const string _httpMethodTemplate = $"%{nameof(_httpMethodTemplate)}%";
	private const string _endpointTemplate = $$"""
		public async Task<HttpResponseMessage> {{_methodNameTemplate}}({{_bodyParameterTypeTemplate}} {{_bodyParameterTemplate}})
		{
			return await SendRequest(new HttpMethod("{{_httpMethodTemplate}}"), $"{{_apiRouteTemplate}}", JsonContent.Create({{_bodyParameterTemplate}}));
		}

		""";

	private readonly Parameter _bodyParameter;

	public PostEndpoint(string methodName, string apiRoute, Parameter bodyParameter)
		: base(HttpMethod.Post, methodName, apiRoute)
	{
		_bodyParameter = bodyParameter;
	}

	public override string Build()
	{
		string bodyParameterStr = _bodyParameter.ToString();
		string bodyParameterType = bodyParameterStr[..bodyParameterStr.IndexOf(' ')];
		string bodyParameter = bodyParameterStr[(bodyParameterStr.IndexOf(' ') + 1)..];

		return _endpointTemplate
			.Replace(_methodNameTemplate, MethodName)
			.Replace(_bodyParameterTypeTemplate, bodyParameterType)
			.Replace(_bodyParameterTemplate, bodyParameter)
			.Replace(_httpMethodTemplate, HttpMethod.ToString())
			.Replace(_apiRouteTemplate, ApiRoute);
	}
}
