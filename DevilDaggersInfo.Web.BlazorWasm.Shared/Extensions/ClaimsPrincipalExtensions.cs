using System.Security.Claims;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
	public static string? GetName(this ClaimsPrincipal claimsPrincipal)
		=> claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

	public static List<string> GetRoles(this ClaimsPrincipal claimsPrincipal)
		=> claimsPrincipal.FindAll(ClaimTypes.Role).Select(c => c.Value).Where(s => s != null).ToList();
}
