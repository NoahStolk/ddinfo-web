using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

		public List<ModData> Binaries { get; } = new();

		public long FileSize { get; private set; }

		public ActionResult? OnGet()
		{
			AssetMod = _dbContext.AssetMods
				.Include(am => am.PlayerAssetMods)
					.ThenInclude(pam => pam.Player)
				.FirstOrDefault(am => am.Name == HttpContext.Request.Query["mod"].ToString());
			if (AssetMod == null)
				return RedirectToPage("Mods");

			IsHostedOnDdInfo = string.IsNullOrWhiteSpace(AssetMod.Url);

			if (IsHostedOnDdInfo)
			{
				string zipPath = Path.Combine(_env.WebRootPath, "mods", $"{AssetMod.Name}.zip");
				if (!Io.File.Exists(zipPath))
					return RedirectToPage("Mods");

				using FileStream fs = new(zipPath, FileMode.Open);
				using ZipArchive archive = new(fs);
				foreach (ZipArchiveEntry entry in archive.Entries)
				{
					byte[] extractedContents = new byte[entry.Length];

					using Stream stream = entry.Open();
					stream.Read(extractedContents, 0, extractedContents.Length);

					Binaries.Add(ModData.CreateFromFile(entry.Name, extractedContents));
				}

				FileSize = fs.Length;
			}

			return null;
		}
	}
}
