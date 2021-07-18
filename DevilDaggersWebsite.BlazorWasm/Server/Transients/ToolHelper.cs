using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.Tools;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DevilDaggersWebsite.BlazorWasm.Server.Transients
{
	public class ToolHelper : IToolHelper
	{
		public ToolHelper(IWebHostEnvironment environment)
		{
			Tools = JsonConvert.DeserializeObject<List<GetTool>?>(File.ReadAllText(Path.Combine(environment.WebRootPath, "tools", "Tools.json"))) ?? throw new("Could not deserialize tools JSON.");
		}

		public List<GetTool> Tools { get; } = new();

		public GetTool GetToolByName(string name)
		{
			GetTool? tool = Tools.Find(t => t.Name == name);
			if (tool == null)
				throw new($"Could not find tool with name {name}.");
			return tool;
		}
	}
}
