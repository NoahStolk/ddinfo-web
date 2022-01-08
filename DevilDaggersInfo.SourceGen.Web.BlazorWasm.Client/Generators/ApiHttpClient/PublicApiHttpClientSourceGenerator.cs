using DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient.Endpoints;
using DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient.Enums;

namespace DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient;

public static class PublicApiHttpClientSourceGenerator
{
	private const string _usings = $"%{nameof(_usings)}%";
	private const string _endpointMethods = $"%{nameof(_endpointMethods)}%";
	private const string _template = $@"{_usings}

namespace DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;

public partial class PublicApiHttpClient
{{
{_endpointMethods}
}}
";

	public static void Execute()
	{
		ApiHttpClientContext apiHttpClientContext = new();
		apiHttpClientContext.AddUsings("DevilDaggersInfo.Web.BlazorWasm.Client.Utils", "System.Net.Http.Json");
		apiHttpClientContext.AddUsings(ClientType.Public, IncludedDirectory.Dto);
		apiHttpClientContext.AddUsings(ClientType.Public, IncludedDirectory.Enums);
		apiHttpClientContext.AddEndpoints(ClientType.Public);

		List<string> endpointMethods = new();
		foreach (Endpoint endpoint in apiHttpClientContext.Endpoints)
			endpointMethods.Add(endpoint.Build());

		string code = _template
			.Replace(_usings, string.Join(Environment.NewLine, apiHttpClientContext.GetOrderedUsingDirectives()))
			.Replace(_endpointMethods, string.Join(Environment.NewLine, endpointMethods).IndentCode(1));
		File.WriteAllText(Path.Combine(Constants.ClientProjectPath, "HttpClients", "PublicApiHttpClientGenerated.cs"), code.WrapCodeInsideWarningSuppressionDirectives().TrimCode());
	}
}
