using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests;

/// <summary>
/// The Swagger documents are built per API from the controller namespace: ApiOperationProcessor only includes a controller
/// whose namespace ends with <c>Controllers.{ApiName}</c>. A controller in the wrong namespace, or an API folder without an
/// AddSwaggerDocument call in Program.cs, silently disappears from every document. These tests compare the documents with
/// the controller actions the application actually registers.
/// </summary>
[NotInParallel(nameof(SwaggerTests))]
internal sealed partial class SwaggerTests : ApplicationTest
{
	private static readonly string[] _apiNames = ["Main", "Admin", "Dd", "Tools", "Ddae", "Ddse", "DdLive", "DdstatsRust", "Clubber"];

	protected override bool ResetBeforeEachTest => false;

	[Test]
	[MethodDataSource(nameof(ApiNames))]
	public async Task Document_IsServedAsOpenApi3(string apiName)
	{
		using JsonDocument document = await GetDocumentAsync(apiName);

		await Assert.That(document.RootElement.GetProperty("openapi").GetString()).StartsWith("3.");
		await Assert.That(document.RootElement.GetProperty("info").GetProperty("title").GetString()).IsEqualTo($"DevilDaggers.info API ({apiName.ToUpperInvariant()})");
		await Assert.That(document.RootElement.GetProperty("paths").EnumerateObject().Any()).IsTrue().Because($"The {apiName} document has no paths.");
	}

	[Test]
	public async Task EveryControllerAction_IsInTheDocumentOfItsNamespace_AndNoOther()
	{
		Dictionary<string, HashSet<string>> operationsByDocument = new(StringComparer.Ordinal);
		foreach (string apiName in _apiNames)
		{
			using JsonDocument document = await GetDocumentAsync(apiName);
			operationsByDocument[apiName] =
			[
				.. document.RootElement.GetProperty("paths").EnumerateObject()
					.SelectMany(path => path.Value.EnumerateObject().Select(method => Operation(method.Name, path.Name))),
			];
		}

		List<(string Operation, string? ExpectedApi, bool IsHidden)> actions = GetControllerActions();
		await Assert.That(actions.Count).IsGreaterThan(100).Because("A discovery problem must not make this test pass on an empty list.");

		List<string> failures = [];
		foreach ((string operation, string? expectedApi, bool isHidden) in actions)
		{
			string[] documents = [.. operationsByDocument.Where(kvp => kvp.Value.Contains(operation)).Select(kvp => kvp.Key)];
			string appearsIn = $"appears in [{string.Join(", ", documents)}]";
			if (isHidden)
			{
				if (documents.Length > 0)
					failures.Add($"{operation} is excluded with [ApiExplorerSettings(IgnoreApi = true)] but {appearsIn}.");

				continue;
			}

			if (expectedApi == null)
			{
				failures.Add(
					$"{operation} is not in a documented API namespace (Controllers.<ApiName>, for one of {string.Join(", ", _apiNames)}) and {appearsIn}. " +
					"A new API also needs an AddSwaggerDocument call in Program.cs and its name in this test.");
			}
			else if (documents is not [string single] || single != expectedApi)
			{
				failures.Add($"{operation} should only be in the {expectedApi} document but {appearsIn}.");
			}
		}

		await Assert.That(failures).IsEmpty().Because(string.Join(Environment.NewLine, failures));
	}

	[Test]
	public async Task SwaggerUi_IsServed()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("swagger/index.html");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
		await Assert.That(response.Content.Headers.ContentType?.MediaType).IsEqualTo("text/html");
	}

	public static IEnumerable<Func<string>> ApiNames()
	{
		return _apiNames.Select(apiName => (Func<string>)(() => apiName));
	}

	private async Task<JsonDocument> GetDocumentAsync(string apiName)
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"swagger/{apiName.ToUpperInvariant()}/swagger.json");

		string body = await response.Content.ReadAsStringAsync();
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(body);
		return JsonDocument.Parse(body);
	}

	/// <summary>
	/// Every controller action the application routes to, as an operation key, with the API whose document it belongs in
	/// according to its namespace (null when its namespace is not directly under Controllers), and whether it is
	/// deliberately excluded from the documents, as the authentication endpoints are.
	/// </summary>
	private List<(string Operation, string? ExpectedApi, bool IsHidden)> GetControllerActions()
	{
		EndpointDataSource endpointDataSource = App.Services.GetRequiredService<EndpointDataSource>();

		List<(string, string?, bool)> actions = [];
		foreach (RouteEndpoint endpoint in endpointDataSource.Endpoints.OfType<RouteEndpoint>())
		{
			ControllerActionDescriptor? action = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
			if (action == null || endpoint.RoutePattern.RawText == null)
				continue;

			string? controllerNamespace = action.ControllerTypeInfo.Namespace;
			string? expectedApi = _apiNames.FirstOrDefault(apiName => controllerNamespace?.EndsWith($".Controllers.{apiName}", StringComparison.Ordinal) == true);
			bool isHidden = endpoint.Metadata.GetMetadata<IApiDescriptionVisibilityProvider>()?.IgnoreApi == true;

			foreach (string method in endpoint.Metadata.GetMetadata<IHttpMethodMetadata>()?.HttpMethods ?? [])
				actions.Add((Operation(method, endpoint.RoutePattern.RawText), expectedApi, isHidden));
		}

		return actions;
	}

	/// <summary>
	/// Normalizes a method and route so that a route template ("api/x/{id:int}") and a Swagger path ("/api/x/{id}") of the
	/// same operation compare equal.
	/// </summary>
	private static string Operation(string method, string route)
	{
		string path = RouteParameterRegex().Replace(route.TrimStart('/'), "{$1}");
		return $"{method.ToUpperInvariant()} /{path}";
	}

	// Strips constraints, defaults and optional markers from route parameters: {id:int}, {id=1} and {id?} become {id}.
	[GeneratedRegex(@"\{\*{0,2}([^}:=?]+)[^}]*\}")]
	private static partial Regex RouteParameterRegex();
}
