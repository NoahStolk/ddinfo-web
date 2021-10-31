using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Middleware;

public class AdminAuthenticationMiddleware
{
	private static readonly Dictionary<string, string> _roleOverrides = new()
	{
		["custom-leaderboards"] = Roles.CustomLeaderboards,
		["donations"] = Roles.Donations,
		["mods"] = Roles.Mods,
		["mod-screenshots"] = Roles.Mods,
		["players"] = Roles.Players,
		["spawnsets"] = Roles.Spawnsets,
	};

	private readonly RequestDelegate _next;

	public AdminAuthenticationMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public Task InvokeAsync(HttpContext context)
	{
		const string adminRouteStart = "api/admin/";
		const string bearer = "Bearer ";
		const string role = "role: ";

		PathString path = context.Request.Path;
		string pathString = path.ToString();
		if (!pathString.Contains(adminRouteStart))
			return _next(context);

		StringValues? auth = context.Request.Headers["Authorization"];
		string? authString = auth?.ToString();
		if (authString?.StartsWith(bearer) != true)
			throw new UnauthorizedAccessException();

		string token = authString[bearer.Length..];
		ClaimsPrincipal user = token.CreateClaimsPrincipalFromJwtTokenString();
		ClaimsIdentity? identity = user.Identities.FirstOrDefault();
		Claim? roleClaim = identity?.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
		string? roleClaimValueString = roleClaim?.Value?.ToString();
		if (roleClaimValueString == null || !roleClaimValueString.StartsWith(role))
			throw new UnauthorizedAccessException();

		string[] userRoles = roleClaimValueString[role.Length..].Split(',') ?? Array.Empty<string>();

		string endpointRoute = pathString[(pathString.IndexOf(adminRouteStart) + adminRouteStart.Length)..];
		if (endpointRoute.Contains('/'))
			endpointRoute = endpointRoute.Substring(0, endpointRoute.IndexOf('/'));

		string requiredRole = _roleOverrides.ContainsKey(endpointRoute) ? _roleOverrides[endpointRoute] : "Admin";
		if (!userRoles.Contains(requiredRole))
			throw new UnauthorizedAccessException();

		return _next(context);
	}
}
