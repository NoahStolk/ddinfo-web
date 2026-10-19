using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.RegularExpressions;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Admin;

/// <summary>
/// Checks every Admin endpoint's authorization against the endpoints the running application actually registers,
/// rather than against a hand-maintained list, so an endpoint added later is covered automatically. Each test
/// collects every offending endpoint before asserting, so one failure lists all of them.
/// </summary>
[NotInParallel(nameof(AdminAuthorizationTests))]
internal sealed partial class AdminAuthorizationTests : ApplicationTest
{
	// Every role-protected endpoint the Admin controllers declare. Used as a floor, so a routing or discovery problem
	// that silently finds no endpoints cannot make the other tests pass vacuously.
	private const int _minimumEndpointCount = 50;

	private static readonly string[] _allRoles = [Roles.Admin, Roles.CustomLeaderboards, Roles.Mods, Roles.Players, Roles.Spawnsets];

	private readonly Dictionary<string, string> _jwtsByRoles = new(StringComparer.Ordinal);

	[Test]
	public async Task EveryAdminEndpoint_RequiresARole()
	{
		List<AdminEndpoint> endpoints = GetAdminEndpoints();
		await Assert.That(endpoints.Count).IsGreaterThanOrEqualTo(_minimumEndpointCount);

		List<string> unprotected = [.. endpoints.Where(e => e.AllowsAnonymous || e.RoleRequirements.Count == 0).Select(e => e.ToString())];
		await Assert.That(unprotected).IsEmpty().Because($"These endpoints are not restricted to a role:{Environment.NewLine}{string.Join(Environment.NewLine, unprotected)}");
	}

	/// <summary>
	/// Every [Authorize] attribute on an endpoint must be satisfied, so a role attribute on an action does not widen the
	/// one on its controller -- it narrows it to users holding both. That has only ever been a copy-paste mistake here.
	/// </summary>
	[Test]
	public async Task NoAdminEndpoint_RequiresRolesFromMoreThanOneAttribute()
	{
		List<string> stacked = [.. GetAdminEndpoints().Where(e => e.RoleRequirements.Count > 1).Select(e => e.ToString())];
		await Assert.That(stacked).IsEmpty().Because($"These endpoints require roles from several [Authorize] attributes at once:{Environment.NewLine}{string.Join(Environment.NewLine, stacked)}");
	}

	[Test]
	public async Task AnonymousRequest_IsRejectedWith401()
	{
		using HttpClient client = App.CreateApiClient();

		List<string> failures = [];
		foreach (AdminEndpoint endpoint in GetAdminEndpoints())
		{
			HttpStatusCode statusCode = await SendAsync(client, endpoint);
			if (statusCode != HttpStatusCode.Unauthorized)
				failures.Add($"{endpoint} returned {(int)statusCode}");
		}

		await Assert.That(failures).IsEmpty().Because(string.Join(Environment.NewLine, failures));
	}

	/// <summary>
	/// For each [Authorize] attribute on an endpoint, a user holding every role except the ones that attribute accepts
	/// must be refused. Giving the user all other roles is what proves that no unrelated role is accepted by mistake.
	/// </summary>
	[Test]
	public async Task UserWithoutTheRequiredRole_IsRejectedWith403()
	{
		List<string> failures = [];
		foreach (AdminEndpoint endpoint in GetAdminEndpoints())
		{
			foreach (IReadOnlyList<string> requirement in endpoint.RoleRequirements)
			{
				string[] otherRoles = [.. _allRoles.Except(requirement, StringComparer.Ordinal)];
				using HttpClient client = App.CreateApiClient(await GetJwtAsync(otherRoles));

				HttpStatusCode statusCode = await SendAsync(client, endpoint);
				if (statusCode != HttpStatusCode.Forbidden)
					failures.Add($"{endpoint} returned {(int)statusCode} for roles [{string.Join(", ", otherRoles)}]");
			}
		}

		await Assert.That(failures).IsEmpty().Because(string.Join(Environment.NewLine, failures));
	}

	/// <summary>
	/// The request carries no body and placeholder route values, so the action itself may well reject it. All that is
	/// asserted is that authorization lets it through, which proves the role claims in the token are the ones the
	/// policies read.
	/// </summary>
	[Test]
	public async Task UserWithTheRequiredRoles_IsNotRejectedByAuthorization()
	{
		List<string> failures = [];
		foreach (AdminEndpoint endpoint in GetAdminEndpoints())
		{
			// One accepted role from each attribute is the least a user needs.
			string[] roles = [.. endpoint.RoleRequirements.Select(r => r[0]).Distinct(StringComparer.Ordinal)];
			using HttpClient client = App.CreateApiClient(await GetJwtAsync(roles));

			HttpStatusCode statusCode = await SendAsync(client, endpoint);
			if (statusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
				failures.Add($"{endpoint} returned {(int)statusCode} for roles [{string.Join(", ", roles)}]");
		}

		await Assert.That(failures).IsEmpty().Because(string.Join(Environment.NewLine, failures));
	}

	/// <summary>
	/// The donation edit page loads through this endpoint. It used to also require the Spawnsets role, so an admin
	/// without it could list donations but not open one.
	/// </summary>
	[Test]
	public async Task AdminWithoutOtherRoles_CanOpenADonation()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Donor"));
			dbContext.Donations.Add(EntityFixtures.Donation(playerId: 1, amountInEurCents: 500));
		});

		using HttpClient client = App.CreateApiClient(await GetJwtAsync([Roles.Admin]));
		using HttpResponseMessage response = await client.GetAsync("api/admin/donations/1");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
	}

	private List<AdminEndpoint> GetAdminEndpoints()
	{
		EndpointDataSource endpointDataSource = App.Services.GetRequiredService<EndpointDataSource>();

		List<AdminEndpoint> endpoints = [];
		foreach (RouteEndpoint endpoint in endpointDataSource.Endpoints.OfType<RouteEndpoint>())
		{
			string? route = endpoint.RoutePattern.RawText;
			if (route == null || !route.StartsWith("api/admin/", StringComparison.Ordinal))
				continue;

			List<IReadOnlyList<string>> roleRequirements =
			[
				.. endpoint.Metadata.GetOrderedMetadata<IAuthorizeData>()
					.Select(a => a.Roles)
					.OfType<string>()
					.Where(roles => !string.IsNullOrWhiteSpace(roles))
					.Select(roles => (IReadOnlyList<string>)[.. roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)]),
			];

			bool allowsAnonymous = endpoint.Metadata.GetMetadata<IAllowAnonymous>() != null;
			foreach (string method in endpoint.Metadata.GetMetadata<IHttpMethodMetadata>()?.HttpMethods ?? [])
				endpoints.Add(new AdminEndpoint(method, route, roleRequirements, allowsAnonymous));
		}

		return [.. endpoints.OrderBy(e => e.Route, StringComparer.Ordinal).ThenBy(e => e.Method, StringComparer.Ordinal)];
	}

	private static async Task<HttpStatusCode> SendAsync(HttpClient client, AdminEndpoint endpoint)
	{
		// Every route parameter gets the same placeholder. Authorization runs before model binding, so its value only
		// matters for requests that are let through.
		string path = RouteParameterRegex().Replace(endpoint.Route, "1");

		using HttpRequestMessage request = new(new HttpMethod(endpoint.Method), path);
		using HttpResponseMessage response = await client.SendAsync(request);
		return response.StatusCode;
	}

	private async Task<string> GetJwtAsync(string[] roles)
	{
		string key = string.Join(",", roles.Order(StringComparer.Ordinal));
		if (!_jwtsByRoles.TryGetValue(key, out string? jwt))
		{
			jwt = await App.CreateJwtAsync($"authz-{_jwtsByRoles.Count}", roles);
			_jwtsByRoles[key] = jwt;
		}

		return jwt;
	}

	[GeneratedRegex(@"\{[^}]+\}")]
	private static partial Regex RouteParameterRegex();

	private sealed record AdminEndpoint(string Method, string Route, IReadOnlyList<IReadOnlyList<string>> RoleRequirements, bool AllowsAnonymous)
	{
		public override string ToString()
		{
			return $"{Method} {Route}";
		}
	}
}
