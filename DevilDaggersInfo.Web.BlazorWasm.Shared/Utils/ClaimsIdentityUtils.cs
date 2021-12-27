using System.Security.Claims;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;

public static class ClaimsIdentityUtils
{
	public static ClaimsIdentity CreateClaimsIdentity(string name, List<string> roles)
	{
		Claim claimNameIdentifier = new(ClaimTypes.NameIdentifier, name);
		List<Claim> claimRoles = roles.ConvertAll(s => new Claim(ClaimTypes.Role, s));
		List<Claim> allClaims = new() { claimNameIdentifier };
		allClaims.AddRange(claimRoles);
		return new(allClaims, "serverAuth");
	}
}
