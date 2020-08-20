using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Pages
{
	public class SpawnsetModel : PageModel
	{
		private readonly ApplicationDbContext dbContext;
		private readonly IWebHostEnvironment env;

		public SpawnsetModel(ApplicationDbContext dbContext, IWebHostEnvironment env)
		{
			this.dbContext = dbContext;
			this.env = env;
		}

		public string? Query { get; private set; }
		public SpawnsetFile? SpawnsetFile { get; private set; }
		public Spawnset? Spawnset { get; private set; }

		public ActionResult? OnGet()
		{
			SpawnsetFile = dbContext.SpawnsetFiles.Include(sf => sf.Player).FirstOrDefault(sf => sf.Name == HttpContext.Request.Query["spawnset"].ToString());
			if (SpawnsetFile == null)
				return RedirectToPage("Spawnsets");

			if (!Spawnset.TryParse(System.IO.File.ReadAllBytes(Path.Combine(env.WebRootPath, "spawnsets", SpawnsetFile.Name)), out Spawnset spawnset))
				throw new Exception($"Could not parse spawnset '{SpawnsetFile.Name}'.");

			Spawnset = spawnset;

			return null;
		}
	}
}