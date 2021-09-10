namespace DevilDaggersInfo.Web.BlazorWasm.Client.SourceGenerator.Generators;

[Generator]
public class PublicApiHttpClientSourceGenerator : ISourceGenerator
{
	private const string _endpointMethods = $"%{nameof(_endpointMethods)}%";
	private const string _template = $@"using DevilDaggersInfo.Web.BlazorWasm.Client.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardHistory;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardHistoryStatistics;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Leaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardStatistics;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Mods;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Spawnsets;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Tools;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Public;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;

public class PublicApiHttpClient
{{
	private readonly HttpClient _client;

	public PublicApiHttpClient(HttpClient client)
	{{
		_client = client;
	}}

{_endpointMethods}
}}
";

	private const string _returnType = $"%{nameof(_returnType)}%";
	private const string _methodName = $"%{nameof(_methodName)}%";
	private const string _methodParameters = $"%{nameof(_methodParameters)}%";
	private const string _queryParameters = $"%{nameof(_queryParameters)}%";
	private const string _apiRoute = $"%{nameof(_apiRoute)}%";
	private const string _endpointTemplate = $@"public async Task<{_returnType}> {_methodName}({_methodParameters})
{{
	Dictionary<string, object?> queryParameters = new()
	{{
{_queryParameters}
	}};
	return await _client.GetFromJsonAsync<{_returnType}>(UrlBuilderUtils.BuildUrlWithQuery(""{_apiRoute}"", queryParameters)) ?? throw new JsonDeserializationException();
}}";

	public void Initialize(GeneratorInitializationContext context)
	{
		context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
	}

	public void Execute(GeneratorExecutionContext context)
	{
		SyntaxReceiver sr = (SyntaxReceiver)context.SyntaxContextReceiver!;

		List<string> endpointMethods = new();
		foreach (Endpoint endpoint in sr.Endpoints)
		{
			string methodParameters = string.Join(", ", endpoint.RouteParameters.Concat(endpoint.QueryParameters).Select(p => $"{p.Type} {p.Name}").ToList());
			string queryParameters = string.Join(Environment.NewLine, endpoint.QueryParameters.ConvertAll(p => $"{{nameof({p.Name}), {p.Name}}}"));

			endpointMethods.Add(_endpointTemplate
				.Replace(_returnType, endpoint.ReturnType)
				.Replace(_methodName, endpoint.MethodName)
				.Replace(_methodParameters, methodParameters)
				.Replace(_queryParameters, queryParameters)
				.Replace(_apiRoute, endpoint.ApiRoute));
		}

		context.AddSource("PublicApiHttpClient", _template.Replace(_endpointMethods, string.Join(Environment.NewLine, endpointMethods)));
	}

	private class SyntaxReceiver : ISyntaxContextReceiver
	{
		public List<Endpoint> Endpoints { get; } = new();

		public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
		{
			// Find all controllers.
			if (context.Node is ClassDeclarationSyntax classSyntax && classSyntax.BaseList?.Types.Any(bt => bt.GetDisplayStringFromContext(context) == "ControllerBase") == true)
			{
				// Find all methods.
				foreach (MethodDeclarationSyntax method in classSyntax.Members.OfType<MethodDeclarationSyntax>())
				{
					// Skip non-public methods.
					if (!method.Modifiers.Any(m => m.Kind() == SyntaxKind.PublicKeyword))
						continue;

					string? name = context.SemanticModel.GetTypeInfo(method).Type?.Name;
					if (name == null)
						continue;

					List<Parameter> allParameters = method.ParameterList.Parameters
						.Select(p => new { Type = p.Type?.GetDisplayStringFromContext(context), Name = p.GetDisplayStringFromContext(context) })
						.Where(p => p.Type != null && p.Name != null)
						.Select(p => new Parameter(p.Type!, p.Name!))
						.ToList();

					string? returnType = method.ReturnType.GetDisplayStringFromContext(context);
					if (returnType == null)
						continue;

					string? apiRoute = method.GetAttributeValueFromMethod(context, "HttpGet");
					if (apiRoute == null)
						continue;

					List<Parameter> queryParameters = allParameters.Where(p => apiRoute.Contains($"{{{p.Name}}}")).ToList();
					List<Parameter> routeParameters = allParameters.Except(queryParameters).ToList();

					Endpoints.Add(new(name, queryParameters, routeParameters, returnType, apiRoute));
				}
			}
		}
	}

	private class Endpoint
	{
		public Endpoint(string methodName, List<Parameter> queryParameters, List<Parameter> routeParameters, string returnType, string apiRoute)
		{
			MethodName = methodName;
			QueryParameters = queryParameters;
			RouteParameters = routeParameters;
			ReturnType = returnType;
			ApiRoute = apiRoute;
		}

		public string MethodName { get; }
		public List<Parameter> QueryParameters { get; }
		public List<Parameter> RouteParameters { get; }
		public string ReturnType { get; }
		public string ApiRoute { get; }
	}

	private class Parameter
	{
		public Parameter(string type, string name)
		{
			Type = type;
			Name = name;
		}

		public string Type { get; }
		public string Name { get; }
	}
}
