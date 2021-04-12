using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

		public bool IsSelfHosted { get; private set; }

		public ActionResult? OnGet()
		{
			AssetMod = _dbContext.AssetMods
				.Include(am => am.PlayerAssetMods)
					.ThenInclude(pam => pam.Player)
				.FirstOrDefault(am => am.Name == HttpContext.Request.Query["mod"].ToString());
			if (AssetMod == null)
				return RedirectToPage("Mods");

			IsSelfHosted = string.IsNullOrWhiteSpace(AssetMod.Url);

			return null;
		}
	}
}
