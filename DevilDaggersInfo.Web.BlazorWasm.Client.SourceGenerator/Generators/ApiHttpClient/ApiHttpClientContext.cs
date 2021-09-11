using DevilDaggersInfo.Web.BlazorWasm.Client.SourceGenerator.Generators.ApiHttpClient.Enums;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.SourceGenerator.Generators.ApiHttpClient;

internal class ApiHttpClientContext
{
	public ApiHttpClientContext()
	{
		foreach (IncludedDirectory includedDirectory in Constants.IncludedDirectories)
		{
			GlobalUsings.Add(includedDirectory, new());

			Dictionary<ClientType, List<string>> specificUsingsDictionary = new();
			foreach (ClientType clientType in Constants.ClientTypes)
				specificUsingsDictionary.Add(clientType, new());

			SpecificUsings.Add(includedDirectory, specificUsingsDictionary);
		}

		foreach (ClientType clientType in Constants.ClientTypes)
			Endpoints.Add(clientType, new());
	}

	public Dictionary<IncludedDirectory, List<string>> GlobalUsings { get; } = new();

	public Dictionary<IncludedDirectory, Dictionary<ClientType, List<string>>> SpecificUsings { get; } = new();

	public Dictionary<ClientType, List<Endpoint>> Endpoints { get; } = new();

	public void FindUsings()
	{
		foreach (IncludedDirectory includedDirectory in Constants.IncludedDirectories)
		{
			string prefix = $"{Constants.SharedProjectName}.{includedDirectory}";
			string path = Path.Combine(Constants.SharedProjectPath, includedDirectory.ToString());
			foreach (string directory in Directory.GetDirectories(path, "*", SearchOption.AllDirectories).Append(path))
			{
				string? directoryName = directory.TrimStart(path);
				string usingDirective = prefix + directoryName.Replace('\\', '.');
				if (directoryName.Contains(nameof(ClientType.Admin)))
					SpecificUsings[includedDirectory][ClientType.Admin].Add(usingDirective);
				else if (directoryName.Contains(nameof(ClientType.Public)))
					SpecificUsings[includedDirectory][ClientType.Public].Add(usingDirective);
				else
					GlobalUsings[includedDirectory].Add(usingDirective);
			}
		}
	}

	public void FindEndpoints()
	{
		foreach (ClientType clientType in Constants.ClientTypes)
		{
			foreach (string controllerFilePath in Directory.GetFiles(Path.Combine(Constants.ServerProjectPath, "Controllers", clientType.ToString())))
				Endpoints[clientType].AddRange(ExtractEndpoints(CSharpSyntaxTree.ParseText(File.ReadAllText(controllerFilePath))));
		}
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
}
