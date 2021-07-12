using DevilDaggersCore.Extensions;
using System.Security.Claims;

namespace DevilDaggersWebsite.BlazorWasm.Server.Extensions
{
	public static class ClaimsPrincipalExtensions
	{
		public static string GetShortName(this ClaimsPrincipal claimsPrincipal)
		{
			if (claimsPrincipal.Identity?.Name == null)
				return "?";

			return claimsPrincipal.Identity.Name.TrimAfter(4);
		}
	}
}
