using DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient.Endpoints;
using DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient.Enums;

namespace DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient;

public static class PublicApiHttpClientSourceGenerator
{
	private const string _usings = $"%{nameof(_usings)}%";
	private const string _endpointMethods = $"%{nameof(_endpointMethods)}%";
	private const string _template = $@"{_usings}
using DevilDaggersInfo.Web.BlazorWasm.Client.Utils;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;

public partial class PublicApiHttpClient
{{
{_endpointMethods}
}}
";

	public static void Execute()
	{
		ApiHttpClientContext apiHttpClientContext = new();
		apiHttpClientContext.AddUsings(ClientType.Public, IncludedDirectory.Dto);
		apiHttpClientContext.AddUsings(ClientType.Public, IncludedDirectory.Enums);
		apiHttpClientContext.AddEndpoints(ClientType.Public);

		List<string> endpointMethods = new();
		foreach (Endpoint endpoint in apiHttpClientContext.Endpoints)
			endpointMethods.Add(endpoint.Build());

		string code = _template
			.Replace(_usings, string.Join(Environment.NewLine, apiHttpClientContext.Usings.Select(s => s.ToUsingDirective())))
			.Replace(_endpointMethods, string.Join(Environment.NewLine, endpointMethods).Indent(1));
		File.WriteAllText(Path.Combine(Constants.ClientProjectPath, "HttpClients", "PublicApiHttpClientGenerated.cs"), SourceBuilderUtils.WrapInsideWarningSuppressionDirectives(code));
	}
}
