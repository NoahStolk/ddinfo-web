using System;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersWebsite.Pages.Admin
{
    public class TestExceptionModel : AdminPageModel
    {
        public ActionResult OnGet(string password)
		{
			if (!Authenticate(password))
				return RedirectToPage("/Error/404");

			throw new Exception("Test exception");
        }
    }
}