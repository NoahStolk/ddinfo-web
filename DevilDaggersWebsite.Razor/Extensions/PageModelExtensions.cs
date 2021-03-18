using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevilDaggersWebsite.Razor.Extensions
{
	public static class PageModelExtensions
	{
		public static string GetIdentity(this PageModel pageModel)
			=> pageModel.User.Identity?.Name?.Substring(0, 4) ?? "UNKNOWN IDENTITY";
	}
}
