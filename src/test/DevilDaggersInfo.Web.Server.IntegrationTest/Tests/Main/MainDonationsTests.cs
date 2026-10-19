using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using System.Net;
using System.Text.Json;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Main;

/// <summary>
/// The donor list shown on the website. The interesting behaviour is what the repository hides: refunded donations,
/// zero-value donations, and the identity of donors who asked to stay anonymous.
/// </summary>
[NotInParallel(nameof(MainDonationsTests))]
internal sealed class MainDonationsTests : ApplicationTest
{
	private async Task<JsonDocument> GetDonorsAsync()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/donations/donors");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
		return JsonDocument.Parse(await response.Content.ReadAsStringAsync());
	}

	[Test]
	public async Task GetDonors_IsEmptyWithNoDonations()
	{
		using JsonDocument document = await GetDonorsAsync();
		await Assert.That(document.RootElement.GetArrayLength()).IsEqualTo(0);
	}

	[Test]
	public async Task GetDonors_ListsTheDonorAndTheirDonation()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(1, "Generous")));
		await App.SeedAsync(dbContext => dbContext.Donations.Add(EntityFixtures.Donation(playerId: 1, amountInEurCents: 500)));

		using JsonDocument document = await GetDonorsAsync();
		JsonElement root = document.RootElement;

		await Assert.That(root.GetArrayLength()).IsEqualTo(1);
		await Assert.That(root[0].GetProperty("playerId").GetInt32()).IsEqualTo(1);
		await Assert.That(root[0].GetProperty("playerName").GetString()).IsEqualTo("Generous");
		await Assert.That(root[0].GetProperty("donations").GetArrayLength()).IsEqualTo(1);
		await Assert.That(root[0].GetProperty("donations")[0].GetProperty("convertedEuroCentsReceived").GetInt32()).IsEqualTo(500);
	}

	[Test]
	public async Task GetDonors_HidesTheIdentityOfAnonymousDonors()
	{
		await App.SeedAsync(dbContext =>
		{
			PlayerEntity player = EntityFixtures.Player(1, "Shy");
			player.HideDonations = true;
			dbContext.Players.Add(player);
		});
		await App.SeedAsync(dbContext => dbContext.Donations.Add(EntityFixtures.Donation(playerId: 1, amountInEurCents: 500)));

		using JsonDocument document = await GetDonorsAsync();
		JsonElement donor = document.RootElement[0];

		// The donation still counts toward the totals, but the player is not identifiable.
		await Assert.That(donor.GetProperty("playerId").ValueKind).IsEqualTo(JsonValueKind.Null);
		await Assert.That(donor.GetProperty("playerName").GetString()).IsEqualTo("(anonymous)");
		await Assert.That(donor.GetProperty("donations").GetArrayLength()).IsEqualTo(1);
	}

	[Test]
	public async Task GetDonors_ExcludesRefundedDonations()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(1, "Refunded")));
		await App.SeedAsync(dbContext => dbContext.Donations.Add(EntityFixtures.Donation(playerId: 1, amountInEurCents: 500, isRefunded: true)));

		// A player whose only donation was refunded is not a donor at all.
		using JsonDocument document = await GetDonorsAsync();
		await Assert.That(document.RootElement.GetArrayLength()).IsEqualTo(0);
	}

	[Test]
	public async Task GetDonors_ExcludesZeroValueDonations()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(1, "Zero")));
		await App.SeedAsync(dbContext => dbContext.Donations.Add(EntityFixtures.Donation(playerId: 1, amountInEurCents: 0)));

		using JsonDocument document = await GetDonorsAsync();
		await Assert.That(document.RootElement.GetArrayLength()).IsEqualTo(0);
	}

	[Test]
	public async Task GetDonors_OrdersByTotalReceivedDescending()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Small"));
			dbContext.Players.Add(EntityFixtures.Player(2, "Large"));
			dbContext.Players.Add(EntityFixtures.Player(3, "Medium"));
		});
		await App.SeedAsync(dbContext =>
		{
			dbContext.Donations.Add(EntityFixtures.Donation(playerId: 1, amountInEurCents: 100));
			dbContext.Donations.Add(EntityFixtures.Donation(playerId: 2, amountInEurCents: 400));
			dbContext.Donations.Add(EntityFixtures.Donation(playerId: 3, amountInEurCents: 250));
		});

		using JsonDocument document = await GetDonorsAsync();
		JsonElement root = document.RootElement;

		await Assert.That(root.GetArrayLength()).IsEqualTo(3);
		await Assert.That(root[0].GetProperty("playerName").GetString()).IsEqualTo("Large");
		await Assert.That(root[1].GetProperty("playerName").GetString()).IsEqualTo("Medium");
		await Assert.That(root[2].GetProperty("playerName").GetString()).IsEqualTo("Small");
	}

	[Test]
	public async Task GetDonors_SumsMultipleDonationsFromOnePlayerForOrdering()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Repeat"));
			dbContext.Players.Add(EntityFixtures.Player(2, "OneOff"));
		});
		await App.SeedAsync(dbContext =>
		{
			dbContext.Donations.Add(EntityFixtures.Donation(playerId: 1, amountInEurCents: 200));
			dbContext.Donations.Add(EntityFixtures.Donation(playerId: 1, amountInEurCents: 200));
			dbContext.Donations.Add(EntityFixtures.Donation(playerId: 2, amountInEurCents: 300));
		});

		using JsonDocument document = await GetDonorsAsync();
		JsonElement root = document.RootElement;

		// 400 total beats a single 300, and both donations are grouped under one donor.
		await Assert.That(root[0].GetProperty("playerName").GetString()).IsEqualTo("Repeat");
		await Assert.That(root[0].GetProperty("donations").GetArrayLength()).IsEqualTo(2);
		await Assert.That(root[1].GetProperty("playerName").GetString()).IsEqualTo("OneOff");
	}
}
