using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AdminTests
{
	public class BinaryNamesModel : PageModel
	{
		private readonly IWebHostEnvironment _environment;

		public BinaryNamesModel(IWebHostEnvironment env)
			=> _environment = env;

		public List<(string ModName, string BinaryName)> Names { get; set; } = new();

		public void OnGet()
		{
			foreach (string path in Directory.GetFiles(Path.Combine(_environment.WebRootPath, "mods"), "*.zip"))
			{
				string modName = Path.GetFileNameWithoutExtension(path);

				using MemoryStream ms = new(System.IO.File.ReadAllBytes(path));
				using ZipArchive archive = new(ms);
				foreach (ZipArchiveEntry entry in archive.Entries)
					Names.Add((modName, entry.Name));
			}
		}
	}
}
