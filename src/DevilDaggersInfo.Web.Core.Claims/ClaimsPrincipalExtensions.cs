using System.Security.Claims;

namespace DevilDaggersInfo.Web.Core.Claims;

public static class ClaimsPrincipalExtensions
{
	extension(ClaimsPrincipal claimsPrincipal)
	{
		public string? GetName()
		{
			return claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}

		public List<string> GetRoles()
		{
			return [.. claimsPrincipal.FindAll(ClaimTypes.Role).Select(c => c.Value)];
		}

		public int? GetPlayerId()
		{
			return int.TryParse(claimsPrincipal.FindFirst("playerId")?.Value, out int playerId) ? playerId : null;
		}
	}
}
