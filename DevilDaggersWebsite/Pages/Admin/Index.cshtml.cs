using CoreBase.Services;
using DevilDaggersWebsite.Models.PageModels;
using Microsoft.AspNetCore.Mvc;
using NetBase.Utils;
using System.IO;
using System.Text;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class IndexModel : AdminPageModel
	{
		private ICommonObjects _commonObjects;

		public string Password { get; set; }

		public string BansFileContents { get; set; }
		public string DonatorsFileContents { get; set; }
		public string FlagsFileContents { get; set; }
		public string TitlesFileContents { get; set; }

		public IndexModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public ActionResult OnGet(string password)
		{
			if (!Authenticate(password))
				return RedirectToPage("/Error/404");

			Password = password;

			BansFileContents = FileUtils.GetContents(Path.Combine(_commonObjects.Env.WebRootPath, "user", "bans"));
			DonatorsFileContents = FileUtils.GetContents(Path.Combine(_commonObjects.Env.WebRootPath, "user", "donators"));
			FlagsFileContents = FileUtils.GetContents(Path.Combine(_commonObjects.Env.WebRootPath, "user", "flags"));
			TitlesFileContents = FileUtils.GetContents(Path.Combine(_commonObjects.Env.WebRootPath, "user", "titles"));

			return null;
		}

		[HttpPost]
		public void OnPost(string password, string bansFileContents, string donatorsFileContents, string flagsFileContents, string titlesFileContents)
		{
			FileUtils.CreateText(Path.Combine(_commonObjects.Env.WebRootPath, "user", "bans"), bansFileContents, Encoding.UTF8);
			FileUtils.CreateText(Path.Combine(_commonObjects.Env.WebRootPath, "user", "donators"), donatorsFileContents, Encoding.UTF8);
			FileUtils.CreateText(Path.Combine(_commonObjects.Env.WebRootPath, "user", "flags"), flagsFileContents, Encoding.UTF8);
			FileUtils.CreateText(Path.Combine(_commonObjects.Env.WebRootPath, "user", "titles"), titlesFileContents, Encoding.UTF8);

			BansFileContents = bansFileContents;
			DonatorsFileContents = donatorsFileContents;
			FlagsFileContents = flagsFileContents;
			TitlesFileContents = titlesFileContents;
		}
	}
}