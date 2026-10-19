using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.DdstatsRust;

/// <summary>
/// The API used by ddstats-rust, which reads game memory using the marker these endpoints serve.
/// </summary>
[NotInParallel(nameof(DdstatsRustContractTests))]
internal sealed class DdstatsRustContractTests : ApplicationTest
{
	private const string _api = "DdstatsRust";

	[Test]
	public async Task GetIntegration()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/integrations/ddstats-rust");

		await ContractSnapshot.AssertMatchesAsync(response, _api, nameof(GetIntegration));
	}

	[Test]
	[Arguments("Windows")]
	[Arguments("Linux")]
	public async Task GetMarker(string operatingSystem)
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Markers.Add(new MarkerEntity { Name = "WindowsSteam", Value = 0x0025_0DC0 });
			dbContext.Markers.Add(new MarkerEntity { Name = "LinuxSteam", Value = 0x0051_3A40 });
		});

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"api/process-memory/marker?operatingSystem={operatingSystem}");

		await ContractSnapshot.AssertMatchesAsync(response, _api, $"{nameof(GetMarker)}_{operatingSystem}");
	}
}
