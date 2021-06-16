using DevilDaggersWebsite.Dto;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DevilDaggersWebsite.Transients
{
	public class ToolHelper : IToolHelper
	{
		public ToolHelper(IWebHostEnvironment env)
		{
			Tools = JsonConvert.DeserializeObject<List<Tool>?>(File.ReadAllText(Path.Combine(env.WebRootPath, "tools", "Tools.json"))) ?? throw new("Could not deserialize tools JSON.");
		}

		public List<Tool> Tools { get; } = new();

		public Tool GetToolByName(string name)
		{
			Tool? tool = Tools.Find(t => t.Name == name);
			if (tool == null)
				throw new($"Could not find tool with name {name}.");
			return tool;
		}
	}
}
