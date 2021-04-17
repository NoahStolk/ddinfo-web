using DevilDaggersWebsite.Caches.Mod;
using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using Io = System.IO;

namespace DevilDaggersWebsite.Razor.Pages
{
	public class ModModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IWebHostEnvironment _env;

		public ModModel(ApplicationDbContext dbContext, IWebHostEnvironment env)
		{
			_dbContext = dbContext;
			_env = env;
		}

		public string? Query { get; }
		public AssetMod? AssetMod { get; private set; }

		public bool IsHostedOnDdInfo { get; private set; }

		public ModArchiveCacheData ArchiveData { get; private set; } = new();
		public bool ContainsProhibitedAssets { get; private set; }

		public ActionResult? OnGet()
		{
			AssetMod = _dbContext.AssetMods
				.Include(am => am.PlayerAssetMods)
					.ThenInclude(pam => pam.Player)
				.FirstOrDefault(am => am.Name == HttpContext.Request.Query["mod"].ToString());
			if (AssetMod == null)
				return RedirectToPage("Mods");

			string zipPath = Path.Combine(_env.WebRootPath, "mods", $"{AssetMod.Name}.zip");
			IsHostedOnDdInfo = Io.File.Exists(zipPath);
			if (IsHostedOnDdInfo)
			{
				ArchiveData = ModArchiveCache.Instance.GetArchiveDataByFilePath(zipPath);
				ContainsProhibitedAssets = ArchiveData.ModData.Any(md => md.ModAssetData.Any(mad => mad.IsProhibited));
			}

			return null;
		}
	}
}
