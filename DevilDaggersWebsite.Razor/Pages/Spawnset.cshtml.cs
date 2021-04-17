using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Io = System.IO;

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
		public int FileSize { get; private set; }
		public bool HasCustomLeaderboard { get; private set; }

		public ActionResult? OnGet()
		{
			SpawnsetFile = _dbContext.SpawnsetFiles.Include(sf => sf.Player).FirstOrDefault(sf => sf.Name == HttpContext.Request.Query["spawnset"].ToString());
			if (SpawnsetFile == null)
				return RedirectToPage("Spawnsets");

			string path = Io.Path.Combine(_env.WebRootPath, "spawnsets", SpawnsetFile.Name);
			if (!Io.File.Exists(path))
				return RedirectToPage("Spawnsets");

			byte[] fileBytes = Io.File.ReadAllBytes(path);
			if (!Spawnset.TryParse(fileBytes, out Spawnset spawnset))
				throw new($"Could not parse spawnset '{SpawnsetFile.Name}'.");

			Spawnset = spawnset;
			FileSize = fileBytes.Length;
			HasCustomLeaderboard = _dbContext.CustomLeaderboards.Any(cl => cl.SpawnsetFileId == SpawnsetFile.Id && !cl.IsArchived);

			return null;
		}
	}
}
