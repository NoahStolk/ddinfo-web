using DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient.Enums;

namespace DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient;

internal class ApiHttpClientContext
{
	private readonly List<string> _usings = new();
	private readonly List<Endpoint> _endpoints = new();

	public IReadOnlyList<string> Usings => _usings;
	public IReadOnlyList<Endpoint> Endpoints => _endpoints;

	public void Clear()
	{
		_usings.Clear();
		_endpoints.Clear();
	}

	public void AddUsings(ClientType clientType, IncludedDirectory includedDirectory)
	{
		string usingPrefix = $"{Constants.SharedProjectName}.{includedDirectory}";

		_usings.Add(usingPrefix);

		string path = Path.Combine(Constants.SharedProjectPath, includedDirectory.ToString());
		foreach (string directory in Directory.GetDirectories(path, "*", SearchOption.AllDirectories))
		{
			string? directoryName = directory.TrimStart(path);

			if (directoryName.Contains(clientType.ToString()))
				_usings.Add(usingPrefix + directoryName.Replace('\\', '.'));
		}
	}

	public void AddEndpoints(ClientType clientType)
	{
		foreach (string controllerFilePath in Directory.GetFiles(Path.Combine(Constants.ServerProjectPath, "Controllers", clientType.ToString())))
			_endpoints.AddRange(ExtractEndpoints(CSharpSyntaxTree.ParseText(File.ReadAllText(controllerFilePath))));
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

			string? returnType = mds.ReturnType.GetTypeStringForApiHttpClient();
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
}
