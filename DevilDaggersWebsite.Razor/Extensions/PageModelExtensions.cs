using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace DevilDaggersWebsite.Razor.Extensions
{
	public static class PageModelExtensions
	{
		public static string GetIdentity(this PageModel pageModel)
			=> pageModel.User.Identity?.Name?.Substring(0, 4) ?? throw new UnauthorizedAccessException("Not allowed to access this resource.");
	}
}
