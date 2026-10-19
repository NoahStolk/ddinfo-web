using JetBrains.Annotations;
using System.Net;
using System.Text.Json;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;

internal static class ProblemAssertions
{
	/// <summary>
	/// Asserts that the response is a problem response from ExceptionMiddleware with the given status, whose title (the
	/// domain exception's message) contains <paramref name="titleFragment"/>.
	/// </summary>
	[AssertionMethod]
	internal static async Task AssertProblemAsync(HttpResponseMessage response, HttpStatusCode expectedStatus, string titleFragment)
	{
		string body = await response.Content.ReadAsStringAsync();
		await Assert.That(response.StatusCode).IsEqualTo(expectedStatus).Because(body);
		await Assert.That(response.Content.Headers.ContentType?.MediaType).IsEqualTo("application/problem+json");

		using JsonDocument document = JsonDocument.Parse(body);
		await Assert.That(document.RootElement.GetProperty("title").GetString()).Contains(titleFragment);
	}

	/// <summary>
	/// Asserts that the request was rejected by model validation before reaching the action, which produces the
	/// framework's own problem shape with an "errors" member rather than a domain exception's title.
	/// </summary>
	[AssertionMethod]
	internal static async Task AssertValidationProblemAsync(HttpResponseMessage response)
	{
		string body = await response.Content.ReadAsStringAsync();
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest).Because(body);

		using JsonDocument document = JsonDocument.Parse(body);
		await Assert.That(document.RootElement.TryGetProperty("errors", out _)).IsTrue().Because(body);
	}
}
