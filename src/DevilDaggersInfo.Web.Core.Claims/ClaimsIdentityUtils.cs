using System.Security.Claims;

namespace DevilDaggersInfo.Web.Core.Claims;

public static class ClaimsIdentityUtils
{
	public static ClaimsIdentity CreateClaimsIdentity(int id, string name, List<string> roles, int? playerId)
	{
		List<Claim> claimRoles = roles.ConvertAll(s => new Claim(ClaimTypes.Role, s));
		List<Claim> allClaims =
		[
			new(ClaimTypes.NameIdentifier, name),
			new("id", id.ToString()),
		];
		if (playerId.HasValue)
			allClaims.Add(new Claim("playerId", playerId.Value.ToString()));

		allClaims.AddRange(claimRoles);
		return new ClaimsIdentity(allClaims, "serverAuth");
	}
}
