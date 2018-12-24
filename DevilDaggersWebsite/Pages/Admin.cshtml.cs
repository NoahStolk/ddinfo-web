using CoreBase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetBase.Utils;
using System;

namespace DevilDaggersWebsite.Pages
{
	public class AdminModel : PageModel
	{
		private ICommonObjects _commonObjects;

		public string BansFileContents { get; set; }
		public string DonatorsFileContents { get; set; }
		public string FlagsFileContents { get; set; }

		public AdminModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public ActionResult OnGet(string pass)
		{
			string password = $"plol{DateTime.Now.Day + DateTime.Now.Month}";

			if (pass != password)
				return RedirectToPage("Error/404");

			BansFileContents = FileUtils.GetContents(System.IO.Path.Combine(_commonObjects.Env.WebRootPath, "user", "bans"));
			DonatorsFileContents = FileUtils.GetContents(System.IO.Path.Combine(_commonObjects.Env.WebRootPath, "user", "donators"));
			FlagsFileContents = FileUtils.GetContents(System.IO.Path.Combine(_commonObjects.Env.WebRootPath, "user", "flags"));

			return null;
		}
	}
}