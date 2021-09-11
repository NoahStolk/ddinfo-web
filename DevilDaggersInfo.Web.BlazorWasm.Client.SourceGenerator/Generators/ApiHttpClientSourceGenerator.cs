namespace DevilDaggersInfo.Web.BlazorWasm.Client.SourceGenerator.Generators;

[Generator]
public class ApiHttpClientSourceGenerator : ISourceGenerator
{
	private const string _serverProjectName = "DevilDaggersInfo.Web.BlazorWasm.Server";
	private const string _sharedProjectName = "DevilDaggersInfo.Web.BlazorWasm.Shared";
	private const string _serverProjectPath = $@"C:\Users\NOAH\source\repos\DevilDaggersInfo\{_serverProjectName}";
	private const string _sharedProjectPath = $@"C:\Users\NOAH\source\repos\DevilDaggersInfo\{_sharedProjectName}";

	private const string _dtoUsings = $"%{nameof(_dtoUsings)}%";
	private const string _enumUsings = $"%{nameof(_enumUsings)}%";
	private const string _endpointMethods = $"%{nameof(_endpointMethods)}%";
	private const string _template = $@"{_dtoUsings}
{_enumUsings}
using DevilDaggersInfo.Web.BlazorWasm.Client.Utils;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

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
}}
";

	private readonly IncludedDirectory[] _includedDirectories = (IncludedDirectory[])Enum.GetValues(typeof(IncludedDirectory));
	private readonly ClientType[] _clientTypes = (ClientType[])Enum.GetValues(typeof(ClientType));

	private readonly Dictionary<IncludedDirectory, List<string>> _globalUsings = new();
	private readonly Dictionary<IncludedDirectory, Dictionary<ClientType, List<string>>> _specificUsings = new();

	private readonly Dictionary<ClientType, List<Endpoint>> _endpoints = new();

	public ApiHttpClientSourceGenerator()
	{
		foreach (IncludedDirectory includedDirectory in _includedDirectories)
		{
			_globalUsings.Add(includedDirectory, new());

			Dictionary<ClientType, List<string>> specificUsingsDictionary = new();
			foreach (ClientType clientType in _clientTypes)
				specificUsingsDictionary.Add(clientType, new());

			_specificUsings.Add(includedDirectory, specificUsingsDictionary);
		}

		foreach (ClientType clientType in _clientTypes)
			_endpoints.Add(clientType, new());
	}

	private enum IncludedDirectory
	{
		Dto,
		Enums,
	}

	private enum ClientType
	{
		Admin,
		Public,
	}

	public void Initialize(GeneratorInitializationContext context)
	{
#if DEBUG
		if (!Debugger.IsAttached)
			Debugger.Launch();
#endif

		foreach (IncludedDirectory includedDirectory in _includedDirectories)
		{
			string prefix = $"{_sharedProjectName}.{includedDirectory}";
			string path = Path.Combine(_sharedProjectPath, includedDirectory.ToString());
			foreach (string directory in Directory.GetDirectories(path, "*", SearchOption.AllDirectories).Append(path))
			{
				string? directoryName = directory.TrimStart(path);
				string usingDirective = prefix + directoryName.Replace('\\', '.');
				if (directoryName.Contains(nameof(ClientType.Admin)))
					_specificUsings[includedDirectory][ClientType.Admin].Add(usingDirective);
				else if (directoryName.Contains(nameof(ClientType.Public)))
					_specificUsings[includedDirectory][ClientType.Public].Add(usingDirective);
				else
					_globalUsings[includedDirectory].Add(usingDirective);
			}
		}

		foreach (ClientType clientType in _clientTypes)
		{
			foreach (string controllerFilePath in Directory.GetFiles(Path.Combine(_serverProjectPath, "Controllers", clientType.ToString())))
				_endpoints[clientType].AddRange(ExtractEndpoints(CSharpSyntaxTree.ParseText(File.ReadAllText(controllerFilePath))));
		}
	}

	public void Execute(GeneratorExecutionContext context)
	{
		foreach (ClientType clientType in _clientTypes)
			GenerateClient(context, clientType, _endpoints[clientType]);
	}

	private void GenerateClient(GeneratorExecutionContext context, ClientType clientType, List<Endpoint> endpoints)
	{
		List<string> endpointMethods = new();
		foreach (Endpoint endpoint in endpoints)
		{
			string methodParameters = string.Join(", ", endpoint.RouteParameters.Concat(endpoint.QueryParameters).Select(p => $"{p.Type} {p.Name}").ToList());
			string queryParameters = string.Join($",{Environment.NewLine}", endpoint.QueryParameters.ConvertAll(p => $"{{nameof({p.Name}), {p.Name}}}"));

			endpointMethods.Add(_endpointTemplate
				.Replace(_returnType, endpoint.ReturnType)
				.Replace(_methodName, endpoint.MethodName)
				.Replace(_methodParameters, methodParameters)
				.Replace(_queryParameters, queryParameters.Indent(2))
				.Replace(_apiRoute, endpoint.ApiRoute));
		}

		context.AddSource($"{clientType}ApiHttpClientGenerated", _template
			.Replace(_dtoUsings, string.Join(Environment.NewLine, _globalUsings[IncludedDirectory.Dto].Concat(_specificUsings[IncludedDirectory.Dto][clientType]).Select(s => s.ToUsingDirective())))
			.Replace(_enumUsings, string.Join(Environment.NewLine, _globalUsings[IncludedDirectory.Enums].Concat(_specificUsings[IncludedDirectory.Enums][clientType]).Select(s => s.ToUsingDirective())))
			.Replace(_endpointMethods, string.Join(Environment.NewLine, endpointMethods).Indent(1)));
	}

	private static IEnumerable<Endpoint> ExtractEndpoints(SyntaxTree syntaxTree)
	{
		SyntaxNode root = syntaxTree.GetRoot();

		// Find class.
		ClassDeclarationSyntax? cds = (ClassDeclarationSyntax)root.DescendantNodes().FirstOrDefault(sn => sn.IsKind(SyntaxKind.ClassDeclaration));
		if (cds == null)
			yield break;

		// Return if not derived from ControllerBase.
		if (cds.BaseList?.Types.Any(bts => bts.ToString() == "ControllerBase") != true)
			yield break;

		// Find base route.
		AttributeSyntax? routeAttribute = cds.GetAttributeFromMember("Route");
		if (routeAttribute == null)
			yield break;

		string apiRoute = routeAttribute.GetRouteAttributeStringValue();

		// Find all methods.
		foreach (MethodDeclarationSyntax mds in cds.Members.OfType<MethodDeclarationSyntax>())
		{
			// Skip non-public methods.
			if (!mds.Modifiers.Any(st => st.Kind() == SyntaxKind.PublicKeyword))
				continue;

			string methodName = mds.Identifier.ToString();

			List<Parameter> allParameters = mds.ParameterList.Parameters
				.Select(ps => new { Type = ps.Type?.ToString(), Name = ps.Identifier.ToString() })
				.Where(ps => ps.Type != null && ps.Name != null)
				.Select(ps => new Parameter(ps.Type!, ps.Name!))
				.ToList();

			string? returnType = mds.ReturnType.GetTypeStringWithoutActionResult();
			if (returnType == null)
				continue;

			AttributeSyntax? httpGetAttribute = mds.GetAttributeFromMember("HttpGet");
			if (httpGetAttribute == null)
				continue;

			string endpointRoute = httpGetAttribute.GetRouteAttributeStringValue();

			List<Parameter> routeParameters = allParameters.Where(p => endpointRoute.Contains($"{{{p.Name}}}")).ToList();
			List<Parameter> queryParameters = allParameters.Except(routeParameters).ToList();

			yield return new(methodName, routeParameters, queryParameters, returnType, $"{apiRoute}/{endpointRoute}");
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
