using DevilDaggersInfo.CommonSourceGen;
using DevilDaggersInfo.Tool.GenerateClient.Extensions;
using DevilDaggersInfo.Tool.GenerateClient.Generators.Endpoints;
using HttpMethod = DevilDaggersInfo.Tool.GenerateClient.Generators.Enums.HttpMethod;

namespace DevilDaggersInfo.Tool.GenerateClient.Generators;

internal class ApiHttpClientContext
{
	private readonly List<string> _usings = new();
	private readonly List<Endpoint> _endpoints = new();

	public IReadOnlyList<string> Usings => _usings;
	public IReadOnlyList<Endpoint> Endpoints => _endpoints;

	public void AddUsings(params string[] usings) => _usings.AddRange(usings);

	public void AddEndpoints(string controllersSubDirectory)
	{
		foreach (string controllerFilePath in Directory.GetFiles(Path.Combine(Constants.ServerProjectPath, "Controllers", controllersSubDirectory)))
			_endpoints.AddRange(ExtractEndpoints(CSharpSyntaxTree.ParseText(File.ReadAllText(controllerFilePath))));
	}

	private IEnumerable<Endpoint> ExtractEndpoints(SyntaxTree syntaxTree)
	{
		SyntaxNode root = syntaxTree.GetRoot();

		foreach (UsingDirectiveSyntax u in root.DescendantNodes().Where(sn => sn.IsKind(SyntaxKind.UsingDirective)).Cast<UsingDirectiveSyntax>().ToList())
		{
			string ns = u.Name.ToString();
			if ((ns.StartsWith("DevilDaggersInfo.Api") || ns.StartsWith("DevilDaggersInfo.Types")) && !_usings.Contains(ns))
				_usings.Add(ns);
		}

		// Find class.
		ClassDeclarationSyntax? cds = (ClassDeclarationSyntax?)root.DescendantNodes().FirstOrDefault(sn => sn.IsKind(SyntaxKind.ClassDeclaration));
		if (cds == null)
			yield break;

		// Return if not derived from ControllerBase.
		if (cds.BaseList?.Types.Any(bts => bts.ToString() == "ControllerBase") != true)
			yield break;

		// Find base route.
		AttributeSyntax? routeAttribute = cds.GetAttributeFromMember("Route");
		if (routeAttribute == null)
			yield break;

		string controllerRoute = routeAttribute.GetRouteAttributeStringValue();

		// Find all methods.
		foreach (MethodDeclarationSyntax mds in cds.Members.OfType<MethodDeclarationSyntax>())
		{
			// Skip non-public methods.
			if (!mds.Modifiers.Any(st => st.IsKind(SyntaxKind.PublicKeyword)))
				continue;

			string methodName = mds.Identifier.ToString();

			// ! LINQ filters out null values.
			List<Parameter> allParameters = mds.ParameterList.Parameters
				.Select(ps => new { Type = ps.Type?.ToString(), Name = ps.Identifier.ToString() })
				.Where(ps => ps.Type != null)
				.Select(ps => new Parameter(ps.Type!, ps.Name))
				.ToList();

			HttpMethodResult? result = GetHttpMethod(mds);
			if (result == null)
				continue;

			string endpointRoute = result.AttributeSyntax.GetRouteAttributeStringValue();

			List<Parameter> routeParameters = allParameters.Where(p => endpointRoute.Contains($"{{{p.Name}}}")).ToList();
			if (routeParameters.Count > 1)
				throw new NotSupportedException($"Multiple route parameters for endpoint '{methodName}' are not supported: {string.Join(", ", routeParameters)}");

			List<Parameter> nonRouteParameters = allParameters.Except(routeParameters).ToList();
			string fullRoute = endpointRoute.StartsWith('/') ? endpointRoute : $"{controllerRoute}/{endpointRoute}";

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
				HttpMethod.Head => new HeadEndpoint(
					methodName,
					fullRoute,
					routeParameters.FirstOrDefault(),
					nonRouteParameters),
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

		AttributeSyntax? httpPatchAttribute = mds.GetAttributeFromMember("HttpPatch");
		if (httpPatchAttribute != null)
			return new(httpPatchAttribute, HttpMethod.Patch);

		AttributeSyntax? httpDeleteAttribute = mds.GetAttributeFromMember("HttpDelete");
		if (httpDeleteAttribute != null)
			return new(httpDeleteAttribute, HttpMethod.Delete);

		AttributeSyntax? httpHeadAttribute = mds.GetAttributeFromMember("HttpHead");
		if (httpHeadAttribute != null)
			return new(httpHeadAttribute, HttpMethod.Head);

		return null;
	}

	public List<string> GetOrderedUsingDirectives()
	{
		string[] usings = Usings.ToArray();
		Array.Sort(usings, StringComparer.Ordinal);
		return usings.Select(s => s.ToUsingDirective()).ToList();
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
