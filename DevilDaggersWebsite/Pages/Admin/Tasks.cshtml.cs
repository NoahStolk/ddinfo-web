using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class TasksModel : AdminPageModel
	{
        public ActionResult OnGet(string password)
		{
			if (!Authenticate(password))
				return RedirectToPage("/Error/404");

			return null;
		}
    }
}