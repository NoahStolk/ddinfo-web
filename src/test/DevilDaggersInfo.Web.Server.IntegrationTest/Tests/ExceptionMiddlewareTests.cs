using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using System.Net;
using System.Text.Json;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests;

/// <summary>
/// The error contract every API consumer shares. Domain exceptions are translated into RFC 7807 problem responses by
/// ExceptionMiddleware, so the status, the content type and the body shape are asserted across several controllers
/// rather than in any one of them.
/// </summary>
[NotInParallel(nameof(ExceptionMiddlewareTests))]
internal sealed class ExceptionMiddlewareTests : ApplicationTest
{
	[Test]
	[Arguments("api/custom-leaderboards/999", HttpStatusCode.NotFound)]
	[Arguments("api/players/999", HttpStatusCode.NotFound)]
	[Arguments("api/app/custom-leaderboards/999", HttpStatusCode.NotFound)]
	[Arguments("api/app/spawnsets/999/buffer", HttpStatusCode.NotFound)]
	[Arguments("api/app/process-memory/marker?appOperatingSystem=Windows", HttpStatusCode.NotFound)]
	[Arguments("api/app/process-memory/marker?appOperatingSystem=Mac", HttpStatusCode.BadRequest)]
	public async Task DomainException_IsReturnedAsAProblemResponse(string route, HttpStatusCode expected)
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync(route);

		await Assert.That(response.StatusCode).IsEqualTo(expected);
		await Assert.That(response.Content.Headers.ContentType?.MediaType).IsEqualTo("application/problem+json");
		await Assert.That(response.Content.Headers.ContentType?.CharSet).IsEqualTo("utf-8");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

		// The status is repeated in the body, and the exception message becomes the title.
		await Assert.That(document.RootElement.GetProperty("status").GetInt32()).IsEqualTo((int)expected);
		await Assert.That(document.RootElement.GetProperty("title").GetString()).IsNotNull();
	}

	[Test]
	public async Task ModelValidationFailure_IsNotHandledByTheMiddleware()
	{
		using HttpClient client = App.CreateApiClient();

		// A [Range] violation is rejected by [ApiController] before the action runs, so it never reaches the middleware
		// and keeps the framework's own validation problem shape with an "errors" member.
		using HttpResponseMessage response = await client.GetAsync("api/spawnsets?withCustomLeaderboardOnly=false&pageSize=999");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.TryGetProperty("errors", out _)).IsTrue();
	}

	[Test]
	public async Task SuccessfulResponse_IsPlainJson()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/custom-leaderboards/allowed-categories");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		// Only errors are problem responses.
		await Assert.That(response.Content.Headers.ContentType?.MediaType).IsEqualTo("application/json");
	}
}
