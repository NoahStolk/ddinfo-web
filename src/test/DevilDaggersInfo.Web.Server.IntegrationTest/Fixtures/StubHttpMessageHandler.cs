namespace DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;

/// <summary>
/// Fails every request. Registered for the typed HTTP clients so that a test which unexpectedly reaches an external
/// service fails loudly instead of hitting the network.
/// </summary>
internal sealed class StubHttpMessageHandler : HttpMessageHandler
{
	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		throw new InvalidOperationException($"An integration test attempted an outbound HTTP request to '{request.RequestUri}'. Substitute the client instead.");
	}
}
