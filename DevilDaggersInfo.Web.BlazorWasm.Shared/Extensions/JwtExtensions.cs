using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;

public static class JwtExtensions
{
	public static ClaimsPrincipal CreateClaimsPrincipalFromJwtTokenString(this string? token)
	{
		if (token == null)
			return new ClaimsPrincipal();

		JwtSecurityTokenHandler tokenHandler = new();

		// TODO: Validate key.
		JwtSecurityToken jwt = tokenHandler.ReadJwtToken(token);

		List<Claim> claims = new();

		string? name = jwt.Claims.FirstOrDefault(c => c.Type == "unique_name")?.ToString();
		if (name != null)
			claims.Add(new Claim(ClaimTypes.Name, name));

		string? role = jwt.Claims.FirstOrDefault(c => c.Type == "role")?.ToString();
		if (role != null)
			claims.Add(new Claim(ClaimTypes.Role, role));

		ClaimsIdentity identity = new(claims, "JWT");
		return new ClaimsPrincipal(identity);
	}
}
