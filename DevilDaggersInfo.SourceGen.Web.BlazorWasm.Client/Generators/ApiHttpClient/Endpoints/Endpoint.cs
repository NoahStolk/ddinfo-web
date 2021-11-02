namespace DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient.Endpoints;

internal abstract class Endpoint
{
	protected Endpoint(HttpMethod httpMethod, string methodName, string apiRoute)
	{
		HttpMethod = httpMethod;
		MethodName = methodName;
		ApiRoute = apiRoute;
	}

	public HttpMethod HttpMethod { get; }
	public string MethodName { get; }
	public string ApiRoute { get; }

	public abstract string Build();
}
