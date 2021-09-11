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

public class PublicApiHttpClientGenerated
{{
	private readonly HttpClient _client;

	public PublicApiHttpClientGenerated(HttpClient client)
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
#if DEBUG
		if (!Debugger.IsAttached)
			Debugger.Launch();
#endif

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

		context.AddSource("PublicApiHttpClientGenerated", _template.Replace(_endpointMethods, string.Join(Environment.NewLine, endpointMethods)));
	}

	private class SyntaxReceiver : ISyntaxContextReceiver
	{
		public List<Endpoint> Endpoints { get; } = new();

		public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
		{
			// Find all classes.
			if (context.Node is ClassDeclarationSyntax cds)
			{
				// Find all controllers.
				if (cds.BaseList?.Types.Any(bts => bts.ToString() == "ControllerBase") != true)
					return;

				// Find base route.
				AttributeSyntax? routeAttribute = cds.GetAttributeFromMember(context, "Microsoft.AspNetCore.Mvc.RouteAttribute");
				if (routeAttribute == null)
					return;

				string apiRoute = routeAttribute.GetRouteAttributeStringValue();

				// Find all methods.
				foreach (MethodDeclarationSyntax mds in cds.Members.OfType<MethodDeclarationSyntax>())
				{
					// Skip non-public methods.
					if (!mds.Modifiers.Any(st => st.Kind() == SyntaxKind.PublicKeyword))
						continue;

					string methodName = mds.Identifier.ToString();

					List<Parameter> allParameters = mds.ParameterList.Parameters
						.Select(ps => new { Type = ps.Type?.GetDisplayStringFromContext(context), Name = ps.Identifier.ToString() })
						.Where(ps => ps.Type != null && ps.Name != null)
						.Select(ps => new Parameter(ps.Type!, ps.Name!))
						.ToList();

					string? returnType = mds.ReturnType.GetDisplayStringFromContext(context); // TODO: Remove ActionResult.
					if (returnType == null)
						continue;

					AttributeSyntax? httpGetAttribute = mds.GetAttributeFromMember(context, "Microsoft.AspNetCore.Mvc.HttpGetAttribute");
					if (httpGetAttribute == null)
						continue;

					string endpointRoute = httpGetAttribute.GetRouteAttributeStringValue();

					List<Parameter> routeParameters = allParameters.Where(p => endpointRoute.Contains($"{{{p.Name}}}")).ToList();
					List<Parameter> queryParameters = allParameters.Except(routeParameters).ToList();

					Endpoints.Add(new(methodName, routeParameters, queryParameters, returnType, $"{apiRoute}/{endpointRoute}"));
				}
			}
		}
	}

	private class Endpoint
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

	private class Parameter
	{
		public Parameter(string type, string name)
		{
			Type = type;
			Name = name;
		}

		public string Type { get; }
		public string Name { get; }

		public override string ToString()
			=> $"{Type} {Name}";
	}
}
