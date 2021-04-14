using DevilDaggersDiscordBot.Logging;
using DevilDaggersWebsite.Razor.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AssetMods
{
	public class DeleteFileModel : PageModel
	{
		private readonly IWebHostEnvironment _env;

		public DeleteFileModel(IWebHostEnvironment env)
		{
			_env = env;
		}

		public IEnumerable<string> ModFileNames { get; private set; } = Enumerable.Empty<string>();

		public void OnGet()
		{
			ModFileNames = Directory.GetFiles(Path.Combine(_env.WebRootPath, "mods")).Select(p => Path.GetFileName(p));
		}

		public async Task<ActionResult> OnPost(string fileName)
		{
			System.IO.File.Delete(Path.Combine(_env.WebRootPath, "mods", fileName));

			await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"`{this.GetIdentity()}` deleted ASSETMOD file `{fileName}`");

			return RedirectToPage("Index");
		}
	}
}
