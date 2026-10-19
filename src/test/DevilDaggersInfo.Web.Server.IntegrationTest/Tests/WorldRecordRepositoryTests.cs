using DevilDaggersInfo.Web.ApiSpec.Main.WorldRecords;
using DevilDaggersInfo.Web.Server.Domain.Main.Repositories;
using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests;

[NotInParallel(nameof(WorldRecordRepositoryTests))]
internal sealed class WorldRecordRepositoryTests : ApplicationTest
{
	private const double _delta = 0.00001;

	[Test]
	public async Task GetWorldRecords_WithCheater()
	{
		await LeaderboardHistoryFixtures.SeedAsync(App);

		await using AsyncServiceScope scope = App.CreateScope();
		GetWorldRecordDataContainer worldRecordData = await scope.ServiceProvider.GetRequiredService<WorldRecordRepository>().GetWorldRecordDataAsync();

		await AssertCheaterIsExcludedAsync(worldRecordData);
	}

	[Test]
	public async Task Endpoint_ServesWorldRecordsWithCheaterExcluded()
	{
		await LeaderboardHistoryFixtures.SeedAsync(App);

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/world-records");
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		GetWorldRecordDataContainer? worldRecordData = await response.Content.ReadFromJsonAsync<GetWorldRecordDataContainer>();
		ArgumentNullException.ThrowIfNull(worldRecordData);
		await AssertCheaterIsExcludedAsync(worldRecordData);
	}

	private static async Task AssertCheaterIsExcludedAsync(GetWorldRecordDataContainer worldRecordData)
	{
		await Assert.That(worldRecordData.WorldRecordHolders.Count).IsEqualTo(1);
		await Assert.That(worldRecordData.WorldRecordHolders[0].Id).IsEqualTo(1);
		await Assert.That(worldRecordData.WorldRecordHolders[0].WorldRecordCount).IsEqualTo(3);

		await Assert.That(worldRecordData.WorldRecords.Count).IsEqualTo(3);
		await Assert.That(worldRecordData.WorldRecords[0].Entry.Id).IsEqualTo(1);
		await Assert.That(worldRecordData.WorldRecords[0].Entry.Time).IsEqualTo(0.0090).Within(_delta);
		await Assert.That(worldRecordData.WorldRecords[1].Entry.Id).IsEqualTo(1);
		await Assert.That(worldRecordData.WorldRecords[1].Entry.Time).IsEqualTo(0.0095).Within(_delta);
		await Assert.That(worldRecordData.WorldRecords[2].Entry.Id).IsEqualTo(1);
		await Assert.That(worldRecordData.WorldRecords[2].Entry.Time).IsEqualTo(0.0098).Within(_delta);
	}
}
