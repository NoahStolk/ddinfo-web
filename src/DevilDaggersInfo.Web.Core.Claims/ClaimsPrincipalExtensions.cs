using System.Security.Claims;

namespace DevilDaggersInfo.Web.Core.Claims;

public static class ClaimsPrincipalExtensions
{
	public static string? GetName(this ClaimsPrincipal claimsPrincipal)
	{
		return claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
	}

	public static List<string> GetRoles(this ClaimsPrincipal claimsPrincipal)
	{
		return claimsPrincipal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
	}

	public static int? GetPlayerId(this ClaimsPrincipal claimsPrincipal)
	{
		return int.TryParse(claimsPrincipal.FindFirst("playerId")?.Value, out int playerId) ? playerId : null;
	}
}
