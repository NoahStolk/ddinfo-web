using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Transients
{
	public class ToolHelper : IToolHelper
	{
		private readonly ApplicationDbContext _dbContext;

		public ToolHelper(ApplicationDbContext dbContext, IWebHostEnvironment environment)
		{
			_dbContext = dbContext;

			Changelogs = JsonConvert.DeserializeObject<Dictionary<string, List<Dto.ChangelogEntry>>?>(File.ReadAllText(Path.Combine(environment.WebRootPath, "tools", "Changelogs.json"))) ?? throw new("Could not deserialize changelogs.json.");
		}

		public Dictionary<string, List<Dto.ChangelogEntry>> Changelogs { get; } = new();

		public Dto.Tool GetToolByName(string name)
		{
			Tool? toolEntity = _dbContext.Tools.AsNoTracking().FirstOrDefault(t => t.Name == name);
			if (toolEntity == null)
				throw new($"Could not find tool with name {name} in database.");

			return GetToolFromEntity(toolEntity);
		}

		public Dto.Tool GetToolFromEntity(Tool tool) => new()
		{
			Changelog = Changelogs.TryGetValue(tool.Name, out List<Dto.ChangelogEntry>? changelog) ? changelog : new List<Dto.ChangelogEntry>(),
			DisplayName = tool.DisplayName,
			Name = tool.Name,
			VersionNumber = Version.Parse(tool.CurrentVersionNumber),
			VersionNumberRequired = Version.Parse(tool.RequiredVersionNumber),
		};
	}
}
