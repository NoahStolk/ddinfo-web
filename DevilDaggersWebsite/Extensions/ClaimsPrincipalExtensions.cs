using DevilDaggersCore.Extensions;
using System;
using System.Security.Claims;

namespace DevilDaggersWebsite.Extensions
{
	public static class ClaimsPrincipalExtensions
	{
		public static string GetShortName(this ClaimsPrincipal claimsPrincipal)
			=> claimsPrincipal.Identity?.Name?.TrimAfter(4) ?? throw new UnauthorizedAccessException("Not allowed to access this resource.");
	}
}
