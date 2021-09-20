namespace DevilDaggersInfo.Web.BlazorWasm.Client.SourceGenerator.Generators.ApiHttpClient;

internal class Endpoint
{
	public Endpoint(string methodName, List<Parameter> routeParameters, List<Parameter> queryParameters, string returnType, string apiRoute)
	{
		MethodName = methodName;
		QueryParameters = queryParameters;
		RouteParameters = routeParameters;
		ReturnType = returnType;
		ApiRoute = apiRoute;
	}

	public string MethodName { get; }
	public List<Parameter> RouteParameters { get; }
	public List<Parameter> QueryParameters { get; }
	public string ReturnType { get; }
	public string ApiRoute { get; }

	public override string ToString()
		=> $"{ApiRoute} {ReturnType} {MethodName}({string.Join(", ", RouteParameters)} | {string.Join(", ", QueryParameters)})";
}
