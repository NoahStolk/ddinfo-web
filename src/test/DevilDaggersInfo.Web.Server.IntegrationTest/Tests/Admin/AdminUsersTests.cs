using DevilDaggersInfo.Web.ApiSpec.Admin.Users;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Admin;

/// <summary>
/// User accounts managed through the Admin API. Role changes and password resets only take effect for tokens issued
/// afterwards, so their effect is checked by logging in as the target user and using the token that login returns.
/// </summary>
[NotInParallel(nameof(AdminUsersTests))]
internal sealed class AdminUsersTests : ApplicationTest
{
	// EntityFixtures.User hashes this password.
	private const string _fixturePassword = "Integration-test-1";

	private const int _targetUserId = 2;

	// ! Assigned by the before-test hook, which TUnit runs before every test method.
	private HttpClient _client = null!;

	[Before(HookType.Test)]
	public async Task CreateAdminClientAsync()
	{
		// The admin is user 1, the user being managed is user 2.
		_client = App.CreateApiClient(await App.CreateJwtAsync("user-admin", Roles.Admin, Roles.Players));
		await App.SeedAsync(dbContext => dbContext.Users.Add(EntityFixtures.User("target")));
	}

	[After(HookType.Test)]
	public void DisposeAdminClient()
	{
		_client.Dispose();
	}

	private async Task<HttpResponseMessage> LoginAsync(string password)
	{
		using HttpClient anonymous = App.CreateApiClient();
		return await anonymous.PostAsJsonAsync("api/authentication/login", new { name = "target", password });
	}

	private async Task<HttpClient> LoginAsTargetAsync(string password = _fixturePassword)
	{
		using HttpResponseMessage login = await LoginAsync(password);
		await Assert.That(login.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await login.Content.ReadAsStringAsync());

		using JsonDocument document = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
		return App.CreateApiClient(document.RootElement.GetProperty("token").GetString());
	}

	private async Task<HttpResponseMessage> ToggleRoleAsync(string roleName)
	{
		return await _client.PatchAsJsonAsync($"api/admin/users/{_targetUserId}/toggle-role", new ToggleRole { RoleName = roleName });
	}

	[Test]
	public async Task ToggleRole_GrantsAndRevokesAccessForNewTokens()
	{
		using (HttpResponseMessage granted = await ToggleRoleAsync(Roles.Mods))
			await Assert.That(granted.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using (HttpClient target = await LoginAsTargetAsync())
		using (HttpResponseMessage mods = await target.GetAsync("api/admin/mods"))
			await Assert.That(mods.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using (HttpResponseMessage revoked = await ToggleRoleAsync(Roles.Mods))
			await Assert.That(revoked.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using HttpClient afterRevoke = await LoginAsTargetAsync();
		using HttpResponseMessage forbidden = await afterRevoke.GetAsync("api/admin/mods");
		await Assert.That(forbidden.StatusCode).IsEqualTo(HttpStatusCode.Forbidden);
	}

	[Test]
	public async Task ToggleRole_IsNotFoundForAnUnknownRole()
	{
		using HttpResponseMessage response = await ToggleRoleAsync("Superuser");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}

	[Test]
	public async Task ToggleRole_IsNotFoundForAnUnknownUser()
	{
		using HttpResponseMessage response = await _client.PatchAsJsonAsync("api/admin/users/999/toggle-role", new ToggleRole { RoleName = Roles.Mods });

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}

	[Test]
	public async Task AssignPlayer_LinksThePlayerForTheProfileEndpoint()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(7, "Linked player")));

		using (HttpResponseMessage assigned = await _client.PutAsJsonAsync($"api/admin/users/{_targetUserId}/assign-player", new AssignPlayer { PlayerId = 7 }))
			await Assert.That(assigned.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await assigned.Content.ReadAsStringAsync());

		using HttpClient target = await LoginAsTargetAsync();
		using HttpResponseMessage profile = await target.GetAsync("api/players/7/profile");
		await Assert.That(profile.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await profile.Content.ReadAsStringAsync());
	}

	[Test]
	public async Task AssignPlayer_RejectsAPlayerLinkedToAnotherUser()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(7, "Taken")));
		await App.SeedAsync(dbContext => dbContext.Users.Add(EntityFixtures.User("owner", playerId: 7)));

		using HttpResponseMessage response = await _client.PutAsJsonAsync($"api/admin/users/{_targetUserId}/assign-player", new AssignPlayer { PlayerId = 7 });
		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "Player with ID '7' is already linked.");
	}

	[Test]
	public async Task AssignPlayer_WithZero_Unlinks()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(7, "Linked player")));
		using (HttpResponseMessage assigned = await _client.PutAsJsonAsync($"api/admin/users/{_targetUserId}/assign-player", new AssignPlayer { PlayerId = 7 }))
			await Assert.That(assigned.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using (HttpResponseMessage unlinked = await _client.PutAsJsonAsync($"api/admin/users/{_targetUserId}/assign-player", new AssignPlayer { PlayerId = 0 }))
			await Assert.That(unlinked.StatusCode).IsEqualTo(HttpStatusCode.OK);

		await using AsyncServiceScope scope = App.CreateScope();
		UserEntity user = await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Users.SingleAsync(u => u.Id == _targetUserId);
		await Assert.That(user.PlayerId).IsNull();
	}

	[Test]
	public async Task ResetPassword_ReplacesTheUsersPassword()
	{
		const string newPassword = "Brand-new-password-2";

		using (HttpResponseMessage reset = await _client.PutAsJsonAsync($"api/admin/users/{_targetUserId}/reset-password", new ResetPassword { NewPassword = newPassword }))
			await Assert.That(reset.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await reset.Content.ReadAsStringAsync());

		using (HttpResponseMessage oldPassword = await LoginAsync(_fixturePassword))
			await Assert.That(oldPassword.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

		using HttpClient target = await LoginAsTargetAsync(newPassword);
	}

	[Test]
	public async Task ResetPassword_RejectsAPasswordThatBreaksThePolicy()
	{
		using HttpResponseMessage response = await _client.PutAsJsonAsync($"api/admin/users/{_targetUserId}/reset-password", new ResetPassword { NewPassword = "short" });

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest).Because(await response.Content.ReadAsStringAsync());

		// The old password still works.
		using HttpClient target = await LoginAsTargetAsync();
	}

	[Test]
	public async Task Delete_RemovesTheUserAndTheirRoles()
	{
		using (HttpResponseMessage granted = await ToggleRoleAsync(Roles.Mods))
			await Assert.That(granted.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using (HttpResponseMessage deleted = await _client.DeleteAsync($"api/admin/users/{_targetUserId}"))
			await Assert.That(deleted.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using (HttpResponseMessage login = await LoginAsync(_fixturePassword))
			await Assert.That(login.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

		await using AsyncServiceScope scope = App.CreateScope();
		await Assert.That(await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().UserRoles.CountAsync(ur => ur.UserId == _targetUserId)).IsEqualTo(0);
	}

	[Test]
	public async Task Delete_IsNotFoundForAnUnknownUser()
	{
		using HttpResponseMessage response = await _client.DeleteAsync("api/admin/users/999");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}
}
