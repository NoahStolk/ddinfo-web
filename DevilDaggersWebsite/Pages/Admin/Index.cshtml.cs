using CoreBase3.Services;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using NetBase.Utils;
using System;
using System.IO;
using System.Text;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class IndexModel : AdminPageModel
	{
		private readonly ICommonObjects commonObjects;

		public string Password { get; set; }

		public string BansFileContents { get; set; }
		public string FlagsFileContents { get; set; }
		public string AssetModsFileContents { get; set; }
		public string SettingsFileContents { get; set; }
		public string TitlesFileContents { get; set; }

		public IndexModel(ICommonObjects commonObjects)
		{
			this.commonObjects = commonObjects;
		}

		public ActionResult OnGet(string password)
		{
			if (!Authenticate(password))
				return RedirectToPage("/Error/404");

			Password = password;

			BansFileContents = FileUtils.GetContents(Path.Combine(commonObjects.Env.WebRootPath, "user", "bans"), Encoding.Default);
			FlagsFileContents = FileUtils.GetContents(Path.Combine(commonObjects.Env.WebRootPath, "user", "flags"), Encoding.Default);
			AssetModsFileContents = FileUtils.GetContents(Path.Combine(commonObjects.Env.WebRootPath, "user", "mods"), Encoding.Default);
			SettingsFileContents = FileUtils.GetContents(Path.Combine(commonObjects.Env.WebRootPath, "user", "settings"), Encoding.Default);
			TitlesFileContents = FileUtils.GetContents(Path.Combine(commonObjects.Env.WebRootPath, "user", "titles"), Encoding.Default);

			return null;
		}

		public void OnPost(string password, string bansFileContents, string flagsFileContents, string assetModsFileContents, string settingsFileContents, string titlesFileContents)
		{
			if (!Authenticate(password))
				throw new Exception("Unauthorized");

			FileUtils.CreateText(Path.Combine(commonObjects.Env.WebRootPath, "user", "bans"), bansFileContents, Encoding.UTF8);
			FileUtils.CreateText(Path.Combine(commonObjects.Env.WebRootPath, "user", "flags"), flagsFileContents, Encoding.UTF8);
			FileUtils.CreateText(Path.Combine(commonObjects.Env.WebRootPath, "user", "mods"), assetModsFileContents, Encoding.UTF8);
			FileUtils.CreateText(Path.Combine(commonObjects.Env.WebRootPath, "user", "settings"), settingsFileContents, Encoding.UTF8);
			FileUtils.CreateText(Path.Combine(commonObjects.Env.WebRootPath, "user", "titles"), titlesFileContents, Encoding.UTF8);

			BansFileContents = bansFileContents;
			FlagsFileContents = flagsFileContents;
			AssetModsFileContents = assetModsFileContents;
			SettingsFileContents = settingsFileContents;
			TitlesFileContents = titlesFileContents;
		}
	}
}