using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AdminTests
{
	public class TestExceptionModel : PageModel
	{
		public void OnGet()
		{
			throw new Exception("Admin test exception", new Exception("Inner exception message", new Exception("Another inner exception message")));
		}
	}
}
