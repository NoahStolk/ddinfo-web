using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Main;

/// <summary>
/// Registration, login and credential changes for the admin portal. These run over HTTP so the bcrypt hashing, the JWT
/// issuing and the token round-trip are all exercised together.
/// </summary>
[NotInParallel(nameof(MainAuthenticationTests))]
internal sealed class MainAuthenticationTests : ApplicationTest
{
	// Satisfies the password policy: at least one digit, one lower, one upper, 12 to 40 characters.
	private const string _password = "Valid-Password-1";

	private async Task<HttpResponseMessage> RegisterAsync(HttpClient client, string name, string password, string? repeated = null)
	{
		return await client.PostAsJsonAsync("api/authentication/register", new
		{
			name,
			password,
			passwordRepeated = repeated ?? password,
		});
	}

	[Test]
	public async Task Register_ThenLogin_ReturnsAUsableToken()
	{
		using HttpClient client = App.CreateApiClient();

		using (HttpResponseMessage registration = await RegisterAsync(client, "newuser", _password))
		{
			await Assert.That(registration.StatusCode).IsEqualTo(HttpStatusCode.OK);
		}

		string token;
		using (HttpResponseMessage login = await client.PostAsJsonAsync("api/authentication/login", new { name = "newuser", password = _password }))
		{
			await Assert.That(login.StatusCode).IsEqualTo(HttpStatusCode.OK);

			using JsonDocument document = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
			await Assert.That(document.RootElement.GetProperty("name").GetString()).IsEqualTo("newuser");
			token = document.RootElement.GetProperty("token").GetString() ?? throw new InvalidOperationException("No token.");
		}

		// The token the server just issued must authenticate against its own authenticate endpoint.
		using HttpResponseMessage authenticate = await client.PostAsJsonAsync("api/authentication/authenticate", new { jwt = token });
		await Assert.That(authenticate.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument authDocument = JsonDocument.Parse(await authenticate.Content.ReadAsStringAsync());
		await Assert.That(authDocument.RootElement.GetProperty("name").GetString()).IsEqualTo("newuser");
		await Assert.That(authDocument.RootElement.GetProperty("roleNames").GetArrayLength()).IsEqualTo(0);
	}

	[Test]
	public async Task Authenticate_ReportsTheUsersRoles()
	{
		string jwt = await App.CreateJwtAsync("roled", Roles.Admin, Roles.Mods);

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.PostAsJsonAsync("api/authentication/authenticate", new { jwt });

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		List<string?> roles = [.. document.RootElement.GetProperty("roleNames").EnumerateArray().Select(e => e.GetString())];

		await Assert.That(roles.Count).IsEqualTo(2);
		await Assert.That(roles).Contains(Roles.Admin);
		await Assert.That(roles).Contains(Roles.Mods);
	}

	[Test]
	public async Task Authenticate_RejectsAGarbageToken()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.PostAsJsonAsync("api/authentication/authenticate", new { jwt = "not-a-token" });

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("title").GetString()).IsEqualTo("Failed to authenticate. The token is invalid.");
	}

	[Test]
	public async Task Login_RejectsAWrongPassword()
	{
		using HttpClient client = App.CreateApiClient();
		using (HttpResponseMessage registration = await RegisterAsync(client, "someone", _password))
		{
			await Assert.That(registration.StatusCode).IsEqualTo(HttpStatusCode.OK);
		}

		using HttpResponseMessage login = await client.PostAsJsonAsync("api/authentication/login", new { name = "someone", password = "Wrong-Password-9" });

		await Assert.That(login.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

		using JsonDocument document = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("title").GetString()).IsEqualTo("Username or password is incorrect.");
	}

	[Test]
	public async Task Login_RejectsAnUnknownUserWithTheSameMessageAsAWrongPassword()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage login = await client.PostAsJsonAsync("api/authentication/login", new { name = "nobody", password = _password });

		await Assert.That(login.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

		// Deliberately indistinguishable from a wrong password, so the response does not confirm which usernames exist.
		using JsonDocument document = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("title").GetString()).IsEqualTo("Username or password is incorrect.");
	}

	[Test]
	public async Task Register_RejectsMismatchedPasswords()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await RegisterAsync(client, "mismatch", _password, "Different-Password-2");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
	}

	[Test]
	[Arguments("short1A")]
	[Arguments("nodigitsupper")]
	[Arguments("nouppercase123")]
	[Arguments("NOLOWERCASE123")]
	public async Task Register_EnforcesThePasswordPolicy(string password)
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await RegisterAsync(client, "policy", password);

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
	}

	[Test]
	public async Task Register_RejectsADuplicateName()
	{
		using HttpClient client = App.CreateApiClient();

		using (HttpResponseMessage first = await RegisterAsync(client, "taken", _password))
		{
			await Assert.That(first.StatusCode).IsEqualTo(HttpStatusCode.OK);
		}

		using HttpResponseMessage second = await RegisterAsync(client, "taken", _password);
		await Assert.That(second.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
	}

	[Test]
	public async Task UpdatePassword_LetsTheUserLogInWithTheNewPasswordOnly()
	{
		const string newPassword = "Another-Password-3";

		using HttpClient anonymous = App.CreateApiClient();
		using (HttpResponseMessage registration = await RegisterAsync(anonymous, "changer", _password))
		{
			await Assert.That(registration.StatusCode).IsEqualTo(HttpStatusCode.OK);
		}

		string jwt;
		using (HttpResponseMessage login = await anonymous.PostAsJsonAsync("api/authentication/login", new { name = "changer", password = _password }))
		{
			using JsonDocument document = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
			jwt = document.RootElement.GetProperty("token").GetString() ?? throw new InvalidOperationException("No token.");
		}

		using HttpClient authenticated = App.CreateApiClient(jwt);
		using (HttpResponseMessage update = await authenticated.PostAsJsonAsync("api/authentication/update-password", new
		{
			currentName = "changer",
			currentPassword = _password,
			newPassword,
			passwordRepeated = newPassword,
		}))
		{
			await Assert.That(update.StatusCode).IsEqualTo(HttpStatusCode.OK);
		}

		using (HttpResponseMessage oldLogin = await anonymous.PostAsJsonAsync("api/authentication/login", new { name = "changer", password = _password }))
		{
			await Assert.That(oldLogin.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
		}

		using HttpResponseMessage newLogin = await anonymous.PostAsJsonAsync("api/authentication/login", new { name = "changer", password = newPassword });
		await Assert.That(newLogin.StatusCode).IsEqualTo(HttpStatusCode.OK);
	}

	[Test]
	public async Task UpdatePassword_RequiresAuthentication()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.PostAsJsonAsync("api/authentication/update-password", new
		{
			currentName = "changer",
			currentPassword = _password,
			newPassword = "Another-Password-3",
			passwordRepeated = "Another-Password-3",
		});

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.Unauthorized);
	}

	[Test]
	public async Task UpdateName_RenamesTheAccount()
	{
		using HttpClient anonymous = App.CreateApiClient();
		using (HttpResponseMessage registration = await RegisterAsync(anonymous, "oldname", _password))
		{
			await Assert.That(registration.StatusCode).IsEqualTo(HttpStatusCode.OK);
		}

		string jwt;
		using (HttpResponseMessage login = await anonymous.PostAsJsonAsync("api/authentication/login", new { name = "oldname", password = _password }))
		{
			using JsonDocument document = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
			jwt = document.RootElement.GetProperty("token").GetString() ?? throw new InvalidOperationException("No token.");
		}

		using HttpClient authenticated = App.CreateApiClient(jwt);
		using (HttpResponseMessage update = await authenticated.PostAsJsonAsync("api/authentication/update-name", new
		{
			currentName = "oldname",
			currentPassword = _password,
			newName = "newname",
		}))
		{
			await Assert.That(update.StatusCode).IsEqualTo(HttpStatusCode.OK);
		}

		using HttpResponseMessage renamedLogin = await anonymous.PostAsJsonAsync("api/authentication/login", new { name = "newname", password = _password });
		await Assert.That(renamedLogin.StatusCode).IsEqualTo(HttpStatusCode.OK);
	}

	[Test]
	public async Task UpdateName_RejectsTheSameName()
	{
		using HttpClient anonymous = App.CreateApiClient();
		using (HttpResponseMessage registration = await RegisterAsync(anonymous, "samename", _password))
		{
			await Assert.That(registration.StatusCode).IsEqualTo(HttpStatusCode.OK);
		}

		string jwt;
		using (HttpResponseMessage login = await anonymous.PostAsJsonAsync("api/authentication/login", new { name = "samename", password = _password }))
		{
			using JsonDocument document = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
			jwt = document.RootElement.GetProperty("token").GetString() ?? throw new InvalidOperationException("No token.");
		}

		using HttpClient authenticated = App.CreateApiClient(jwt);
		using HttpResponseMessage update = await authenticated.PostAsJsonAsync("api/authentication/update-name", new
		{
			currentName = "samename",
			currentPassword = _password,
			newName = "samename",
		});

		await Assert.That(update.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

		using JsonDocument updateDocument = JsonDocument.Parse(await update.Content.ReadAsStringAsync());
		await Assert.That(updateDocument.RootElement.GetProperty("title").GetString()).IsEqualTo("The same username was entered.");
	}
}
