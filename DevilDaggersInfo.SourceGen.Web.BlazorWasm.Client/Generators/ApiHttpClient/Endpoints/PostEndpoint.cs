namespace DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient.Endpoints;

internal class PostEndpoint : Endpoint
{
	private const string _methodName = $"%{nameof(_methodName)}%";
	private const string _bodyParameter = $"%{nameof(_bodyParameter)}%";
	private const string _apiRoute = $"%{nameof(_apiRoute)}%";
	private const string _httpMethod = $"%{nameof(_httpMethod)}%";
	private const string _endpointTemplate = $@"public async Task<HttpResponseMessage> {_methodName}({_bodyParameter})
{{
	return await SendRequest(new HttpMethod(""{_httpMethod}""), $""{_apiRoute}"");
}}
";

	public PostEndpoint(string methodName, Parameter bodyParameter, string apiRoute)
		: base(HttpMethod.Post, methodName, apiRoute)
	{
		BodyParameter = bodyParameter;
	}

	public Parameter BodyParameter { get; }

	public override string Build()
	{
		return _endpointTemplate
			.Replace(_methodName, MethodName)
			.Replace(_bodyParameter, BodyParameter.ToString())
			.Replace(_httpMethod, HttpMethod.ToString())
			.Replace(_apiRoute, ApiRoute);
	}
}
