using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using System.Net;
using System.Text.Json;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Tools;

[NotInParallel(nameof(ToolsProcessMemoryTests))]
internal sealed class ToolsProcessMemoryTests : ApplicationTest
{
	[Test]
	[Arguments("Windows", "WindowsSteam", 1234567890L)]
	[Arguments("Linux", "LinuxSteam", 987654321L)]
	public async Task GetMarker_ReturnsTheMarkerForTheOperatingSystem(string appOperatingSystem, string markerName, long value)
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Markers.Add(new MarkerEntity { Name = "WindowsSteam", Value = 1234567890L });
			dbContext.Markers.Add(new MarkerEntity { Name = "LinuxSteam", Value = 987654321L });
		});

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"api/app/process-memory/marker?appOperatingSystem={appOperatingSystem}");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("value").GetInt64()).IsEqualTo(value);
		await Assert.That(markerName).IsNotNull();
	}

	[Test]
	public async Task GetMarker_IsNotFoundWhenTheMarkerIsMissing()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/app/process-memory/marker?appOperatingSystem=Windows");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("title").GetString()).IsEqualTo("Marker key 'WindowsSteam' was not found in database.");
	}

	[Test]
	public async Task GetMarker_RejectsAnUnsupportedOperatingSystem()
	{
		using HttpClient client = App.CreateApiClient();

		// Mac is a declared enum member with no marker mapping, so the server rejects it rather than 404ing.
		using HttpResponseMessage response = await client.GetAsync("api/app/process-memory/marker?appOperatingSystem=Mac");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("title").GetString()).IsEqualTo("Operating system 'Mac' is not supported.");
	}
}
