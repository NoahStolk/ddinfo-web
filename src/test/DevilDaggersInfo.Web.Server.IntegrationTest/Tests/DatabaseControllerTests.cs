using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using System.Net;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests;

[NotInParallel(nameof(DatabaseControllerTests))]
internal sealed class DatabaseControllerTests : ApplicationTest
{
	/// <summary>
	/// This test class deliberately does not use the bootstrap database, so the schema it runs against is not named
	/// "devildaggers". The endpoint used to hard-code that name and would have reported an empty database here.
	/// </summary>
	[Test]
	public async Task GetDatabaseInfo_ReportsTablesOfTheConfiguredSchema()
	{
		string jwt = await App.CreateJwtAsync("db-admin", Roles.Admin);
		using HttpClient client = App.CreateApiClient(jwt);
		using HttpResponseMessage response = await client.GetAsync("api/admin/database");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		string json = await response.Content.ReadAsStringAsync();
		await Assert.That(json).Contains("Players");
		await Assert.That(json).Contains("SpawnsetFiles");
	}
}
