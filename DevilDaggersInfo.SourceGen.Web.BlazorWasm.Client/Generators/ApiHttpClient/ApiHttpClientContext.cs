using DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient.Endpoints;
using DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient.Enums;
using HttpMethod = DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient.Enums.HttpMethod;

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

			HttpMethodResult? result = GetHttpMethod(mds);
			if (result == null)
				continue;

			string endpointRoute = result.AttributeSyntax.GetRouteAttributeStringValue();

			List<Parameter> routeParameters = allParameters.Where(p => endpointRoute.Contains($"{{{p.Name}}}")).ToList();
			if (routeParameters.Count > 1)
				throw new NotSupportedException($"Multiple route parameters for endpoint '{methodName}' are not supported: {string.Join(", ", routeParameters)}");

			List<Parameter> nonRouteParameters = allParameters.Except(routeParameters).ToList();
			string fullRoute = $"{apiRoute}/{endpointRoute}";

			yield return result.HttpMethod switch
			{
				HttpMethod.Get => new GetEndpoint(
					methodName,
					fullRoute,
					routeParameters.FirstOrDefault(),
					nonRouteParameters,
					mds.ReturnType.GetTypeStringForApiHttpClient()),
				HttpMethod.Post => new PostEndpoint(
					methodName,
					fullRoute,
					nonRouteParameters.Count != 1 ? throw new NotSupportedException($"POST endpoint '{methodName}' must have exactly 1 non-route parameter: {string.Join(", ", nonRouteParameters)}") : nonRouteParameters[0]),
				HttpMethod.Put => new PutEndpoint(
					methodName,
					fullRoute,
					routeParameters.Count != 1 ? throw new NotSupportedException($"PUT endpoint '{methodName}' must have exactly 1 route parameter: {string.Join(", ", routeParameters)}") : routeParameters[0],
					nonRouteParameters.Count != 1 ? throw new NotSupportedException($"PUT endpoint '{methodName}' must have exactly 1 non-route parameter: {string.Join(", ", nonRouteParameters)}") : nonRouteParameters[0]),
				HttpMethod.Patch => new PatchEndpoint(
					methodName,
					fullRoute,
					routeParameters.Count != 1 ? throw new NotSupportedException($"PATCH endpoint '{methodName}' must have exactly 1 route parameter: {string.Join(", ", routeParameters)}") : routeParameters[0],
					nonRouteParameters.Count != 1 ? throw new NotSupportedException($"PATCH endpoint '{methodName}' must have exactly 1 non-route parameter: {string.Join(", ", nonRouteParameters)}") : nonRouteParameters[0]),
				HttpMethod.Delete => new DeleteEndpoint(
					methodName,
					fullRoute,
					routeParameters.Count != 1 ? throw new NotSupportedException($"DELETE endpoint '{methodName}' must have exactly 1 route parameter: {string.Join(", ", routeParameters)}") : routeParameters[0]),
				_ => throw new NotSupportedException($"Endpoint with HTTP method '{result.HttpMethod}' is not supported."),
			};
		}
	}

	private static HttpMethodResult? GetHttpMethod(MethodDeclarationSyntax mds)
	{
		AttributeSyntax? httpGetAttribute = mds.GetAttributeFromMember("HttpGet");
		if (httpGetAttribute != null)
			return new(httpGetAttribute, HttpMethod.Get);

		AttributeSyntax? httpPostAttribute = mds.GetAttributeFromMember("HttpPost");
		if (httpPostAttribute != null)
			return new(httpPostAttribute, HttpMethod.Post);

		AttributeSyntax? httpPutAttribute = mds.GetAttributeFromMember("HttpPut");
		if (httpPutAttribute != null)
			return new(httpPutAttribute, HttpMethod.Put);

		AttributeSyntax? httpDeleteAttribute = mds.GetAttributeFromMember("HttpDelete");
		if (httpDeleteAttribute != null)
			return new(httpDeleteAttribute, HttpMethod.Delete);

		AttributeSyntax? httpPatchAttribute = mds.GetAttributeFromMember("HttpPatch");
		if (httpPatchAttribute != null)
			return new(httpPatchAttribute, HttpMethod.Patch);

		return null;
	}

	private sealed class HttpMethodResult
	{
		public HttpMethodResult(AttributeSyntax attributeSyntax, HttpMethod httpMethod)
		{
			AttributeSyntax = attributeSyntax;
			HttpMethod = httpMethod;
		}

		public AttributeSyntax AttributeSyntax { get; }
		public HttpMethod HttpMethod { get; }
	}
}
