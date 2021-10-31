namespace DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient;

internal class Endpoint
{
	public Endpoint(HttpMethod httpMethod, string methodName, List<Parameter> routeParameters, List<Parameter> queryParameters, string returnType, string apiRoute)
	{
		HttpMethod = httpMethod;
		MethodName = methodName;
		QueryParameters = queryParameters;
		RouteParameters = routeParameters;
		ReturnType = returnType;
		ApiRoute = apiRoute;
	}

	public HttpMethod HttpMethod { get; }
	public string MethodName { get; }
	public List<Parameter> RouteParameters { get; }
	public List<Parameter> QueryParameters { get; }
	public string ReturnType { get; }
	public string ApiRoute { get; }

	public override string ToString()
		=> $"{HttpMethod} {ApiRoute} {ReturnType} {MethodName}({string.Join(", ", RouteParameters)} | {string.Join(", ", QueryParameters)})";
}
