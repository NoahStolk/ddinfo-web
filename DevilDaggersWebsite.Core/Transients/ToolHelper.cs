using DevilDaggersWebsite.Core.Dto;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DevilDaggersWebsite.Core.Transients
{
	public class ToolHelper
	{
		public ToolHelper(IWebHostEnvironment env)
		{
			Tools = JsonConvert.DeserializeObject<List<Tool>>(File.ReadAllText(Path.Combine(env.WebRootPath, "tools", "Tools.json")));
		}

		public List<Tool> Tools { get; }

		public Tool GetToolByName(string name)
		{
			Tool? tool = Tools.Find(t => t.Name == name);
			if (tool == null)
				throw new Exception($"Could not find tool with name {name}.");
			return tool;
		}
	}
}