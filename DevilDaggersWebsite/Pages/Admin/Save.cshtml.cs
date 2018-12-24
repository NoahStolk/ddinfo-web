using CoreBase.Services;
using DevilDaggersWebsite.Models.PageModels;
using Microsoft.AspNetCore.Mvc;
using NetBase.Utils;
using System.IO;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class SaveModel : AdminPageModel
	{
		private ICommonObjects _commonObjects;

		public SaveModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public ActionResult OnPost(string password, string bans, string donators, string flags)
		{
			if (!Authenticate(password))
				return RedirectToPage("Error/404");

			FileUtils.CreateText(Path.Combine(_commonObjects.Env.WebRootPath, "user", "bans"), bans);
			FileUtils.CreateText(Path.Combine(_commonObjects.Env.WebRootPath, "user", "donators"), donators);
			FileUtils.CreateText(Path.Combine(_commonObjects.Env.WebRootPath, "user", "flags"), flags);

			return RedirectToPage("/Admin/Index");
		}
	}
}