using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Razor.Pages
{
	public class SpawnsetModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IWebHostEnvironment _env;

		public SpawnsetModel(ApplicationDbContext dbContext, IWebHostEnvironment env)
		{
			_dbContext = dbContext;
			_env = env;
		}

		public string? Query { get; }
		public SpawnsetFile? SpawnsetFile { get; private set; }
		public Spawnset? Spawnset { get; private set; }

		public ActionResult? OnGet()
		{
			SpawnsetFile = _dbContext.SpawnsetFiles.Include(sf => sf.Player).FirstOrDefault(sf => sf.Name == HttpContext.Request.Query["spawnset"].ToString());
			if (SpawnsetFile == null)
				return RedirectToPage("Spawnsets");

			if (!Spawnset.TryParse(System.IO.File.ReadAllBytes(Path.Combine(_env.WebRootPath, "spawnsets", SpawnsetFile.Name)), out Spawnset spawnset))
				throw new Exception($"Could not parse spawnset '{SpawnsetFile.Name}'.");

			Spawnset = spawnset;

			return null;
		}
	}
}