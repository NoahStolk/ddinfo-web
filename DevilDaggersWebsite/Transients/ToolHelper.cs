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

			Tools = JsonConvert.DeserializeObject<List<Dto.Tool>?>(File.ReadAllText(Path.Combine(environment.WebRootPath, "tools", "Tools.json"))) ?? throw new("Could not deserialize tools JSON.");
		}

		public List<Dto.Tool> Tools { get; } = new();

		public Dto.Tool GetToolByName(string name)
		{
			Tool? toolEntity = _dbContext.Tools.AsNoTracking().FirstOrDefault(t => t.Name == name);
			if (toolEntity == null)
				throw new($"Could not find tool with name {name} in database.");

			Dto.Tool? tool = Tools.Find(t => t.Name == name);
			if (tool == null)
				throw new($"Could not find tool with name {name} in file system.");

			tool.VersionNumber = Version.Parse(toolEntity.CurrentVersionNumber);
			tool.VersionNumberRequired = Version.Parse(toolEntity.RequiredVersionNumber);

			return tool;
		}
	}
}
