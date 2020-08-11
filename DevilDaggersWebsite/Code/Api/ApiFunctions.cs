using DevilDaggersCore.Spawnsets.Web;
using DevilDaggersCore.Website;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Code.Api
{
	public static class ApiFunctions
	{
		public static bool TryGetSpawnsetPath(IWebHostEnvironment env, string fileName, out string path)
		{
			if (!string.IsNullOrEmpty(fileName) && File.Exists(Path.Combine(env.WebRootPath, "spawnsets", fileName)))
			{
				path = Path.Combine("spawnsets", fileName);
				return true;
			}

			path = string.Empty;
			return false;
		}

		public static IEnumerable<SpawnsetFile> GetSpawnsets(IWebHostEnvironment env, string searchAuthor, string searchName)
		{
			IEnumerable<SpawnsetFile> spawnsetFiles = Directory.GetFiles(Path.Combine(env.WebRootPath, "spawnsets")).Select(p => SpawnsetUtils.CreateSpawnsetFileFromSettingsFile(env, p));

			if (!string.IsNullOrEmpty(searchAuthor))
			{
				searchAuthor = searchAuthor.ToLower();
				spawnsetFiles = spawnsetFiles.Where(sf => sf.Author.ToLower().Contains(searchAuthor));
			}
			if (!string.IsNullOrEmpty(searchName))
			{
				searchName = searchName.ToLower();
				spawnsetFiles = spawnsetFiles.Where(sf => sf.Name.ToLower().Contains(searchName));
			}

			return spawnsetFiles;
		}

		public static bool TryGetToolPath(IWebHostEnvironment env, string toolName, out string fileName, out string path)
		{
			Tool tool = ToolList.Tools.FirstOrDefault(t => t.Name == toolName);
			if (tool != null)
			{
				fileName = $"{toolName}{tool.VersionNumber}.zip";
				path = Path.Combine("tools", toolName, fileName);

				if (!File.Exists(Path.Combine(env.WebRootPath, path)))
					throw new Exception($"Tool file '{path}' does not exist.");

				return true;
			}

			fileName = "";
			path = "";
			return false;
		}
	}
}