using DevilDaggersInfo.CommonSourceGen;
using DevilDaggersInfo.Tool.GenerateClient.Generators.Endpoints;
using DevilDaggersInfo.Tool.GenerateClient.Generators.Enums;

namespace DevilDaggersInfo.Tool.GenerateClient.Generators;

public static class AdminApiHttpClientSourceGenerator
{
	private const string _usings = $"%{nameof(_usings)}%";
	private const string _endpointMethods = $"%{nameof(_endpointMethods)}%";
	private const string _template = $@"{_usings}

namespace DevilDaggersInfo.Web.Client.HttpClients;

public partial class AdminApiHttpClient
{{
{_endpointMethods}
}}
";

	public static void Execute()
	{
		ApiHttpClientContext apiHttpClientContext = new();
		apiHttpClientContext.AddUsings("DevilDaggersInfo.Web.Client.Utils", "System.Net.Http.Json");
		apiHttpClientContext.AddUsings(ClientType.Admin, IncludedDirectory.Dto);
		apiHttpClientContext.AddUsings(ClientType.Admin, IncludedDirectory.Enums);
		apiHttpClientContext.AddEndpoints(ClientType.Admin);

		List<string> endpointMethods = new();
		foreach (Endpoint endpoint in apiHttpClientContext.Endpoints)
			endpointMethods.Add(endpoint.Build());

		string code = _template
			.Replace(_usings, string.Join(Environment.NewLine, apiHttpClientContext.GetOrderedUsingDirectives()))
			.Replace(_endpointMethods, string.Join(Environment.NewLine, endpointMethods).IndentCode(1));
		File.WriteAllText(Path.Combine(Constants.ClientProjectPath, "HttpClients", "AdminApiHttpClientGenerated.cs"), code.WrapCodeInsideWarningSuppressionDirectives().TrimCode());
	}
}
