using System.Security.Claims;

namespace DevilDaggersInfo.Web.Core.Claims;

public static class ClaimsPrincipalExtensions
{
	public static string? GetName(this ClaimsPrincipal claimsPrincipal)
		=> claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

	public static List<string> GetRoles(this ClaimsPrincipal claimsPrincipal)
		=> claimsPrincipal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

	public static int? GetPlayerId(this ClaimsPrincipal claimsPrincipal)
		=> int.TryParse(claimsPrincipal.FindFirst("playerId")?.Value, out int playerId) ? playerId : null;
}
