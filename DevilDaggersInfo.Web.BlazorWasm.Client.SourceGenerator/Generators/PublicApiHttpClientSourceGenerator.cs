using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

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
		SyntaxReceiver rx = (SyntaxReceiver)context.SyntaxContextReceiver!;
		foreach (Endpoint endpoint in rx.Endpoints)
		{
			//string source = SourceFileFromMustachePath(name, template, hash);
		}

		context.AddSource("PublicApiHttpClient", "");
	}

	private class SyntaxReceiver : ISyntaxContextReceiver
	{
		public List<Endpoint> Endpoints { get; } = new();

		public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
		{
			// Find all controllers.
			if (context.Node is ClassDeclarationSyntax classSyntax && classSyntax.BaseList?.Types.Any(bt => context.SemanticModel.GetTypeInfo(bt).Type?.ToDisplayString() == "ControllerBase") == true)
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

					List<Parameter> queryParameters = new();
					List<Parameter> routeParameters = new();
					string? apiRoute = null;

					Endpoints.Add(new(name, queryParameters, routeParameters, method.ReturnType, apiRoute));
				}
			}
		}
	}

	private class Endpoint
	{
		public Endpoint(string name, List<Parameter> queryParameters, List<Parameter> routeParameters, Type returnType, string apiRoute)
		{
			Name = name;
			QueryParameters = queryParameters;
			RouteParameters = routeParameters;
			ReturnType = returnType;
			ApiRoute = apiRoute;
		}

		public string Name { get; }
		public List<Parameter> QueryParameters { get; }
		public List<Parameter> RouteParameters { get; }
		public Type ReturnType { get; }
		public string ApiRoute { get; }
	}

	private class Parameter
	{
		public Parameter(Type type, string name)
		{
			Type = type;
			Name = name;
		}

		public Type Type { get; }
		public string Name { get; }
	}
}
