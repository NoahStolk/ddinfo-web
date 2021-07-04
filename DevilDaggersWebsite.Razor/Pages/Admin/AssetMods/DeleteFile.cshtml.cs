using DevilDaggersWebsite.Razor.PageModels;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AssetMods
{
	public class DeleteFileModel : AbstractAdminPageModel
	{
		private readonly IWebHostEnvironment _environment;

		public DeleteFileModel(IWebHostEnvironment environment)
		{
			_environment = environment;
		}

		public IEnumerable<string> ModFileNames { get; private set; } = Enumerable.Empty<string>();

		public void OnGet()
		{
			ModFileNames = Directory.GetFiles(Path.Combine(_environment.WebRootPath, "mods")).Select(p => Path.GetFileName(p));
		}
	}
}
