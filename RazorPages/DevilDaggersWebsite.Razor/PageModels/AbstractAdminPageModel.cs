using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public abstract class AbstractAdminPageModel : PageModel
	{
		public string GetIdentity()
			=> User.Identity?.Name?.Substring(0, 4) ?? throw new UnauthorizedAccessException("Not allowed to access this resource.");
	}
}
