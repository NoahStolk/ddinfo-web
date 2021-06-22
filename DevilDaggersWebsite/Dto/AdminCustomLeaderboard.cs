using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Dto
{
	public class AdminCustomLeaderboard : IAdminDto
	{
		public CustomLeaderboardCategory Category { get; init; }
		public int SpawnsetFileId { get; init; }

		[Range(10000, 15000000)]
		public int TimeBronze { get; init; }

		[Range(10000, 15000000)]
		public int TimeSilver { get; init; }

		[Range(10000, 15000000)]
		public int TimeGolden { get; init; }

		[Range(10000, 15000000)]
		public int TimeDevil { get; init; }

		[Range(10000, 15000000)]
		public int TimeLeviathan { get; init; }

		public bool IsArchived { get; init; }

		public Dictionary<string, string> Log()
		{
			Dictionary<string, string> dictionary = new();
			dictionary.Add(nameof(Category), Category.ToString());
			dictionary.Add(nameof(SpawnsetFileId), SpawnsetFileId.ToString());
			dictionary.Add(nameof(TimeBronze), TimeBronze.ToString());
			dictionary.Add(nameof(TimeSilver), TimeSilver.ToString());
			dictionary.Add(nameof(TimeGolden), TimeGolden.ToString());
			dictionary.Add(nameof(TimeDevil), TimeDevil.ToString());
			dictionary.Add(nameof(TimeLeviathan), TimeLeviathan.ToString());
			dictionary.Add(nameof(IsArchived), IsArchived.ToString());
			return dictionary;
		}

		public bool ValidateGlobal(ModelStateDictionary modelState, ApplicationDbContext dbContext, IWebHostEnvironment env)
		{
			string? invalidDaggerTime = GetInvalidDaggerTime();
			if (invalidDaggerTime != null)
			{
				modelState.AddModelError($"AdminDto.{invalidDaggerTime}", "The dagger times are incorrect.");
				return false;
			}

			Entities.SpawnsetFile? spawnsetFile = dbContext.SpawnsetFiles.FirstOrDefault(sf => sf.Id == SpawnsetFileId);
			if (spawnsetFile == null)
			{
				modelState.AddModelError($"AdminDto.{nameof(SpawnsetFileId)}", $"A spawnset with ID '{SpawnsetFileId}' does not exist.");
				return false;
			}

			if (!Spawnset.TryParse(File.ReadAllBytes(Path.Combine(env.WebRootPath, "spawnsets", spawnsetFile.Name)), out Spawnset spawnset))
				throw new($"Could not parse survival file '{spawnsetFile.Name}'. Please review the file. Also review how this file ended up in the 'spawnsets' directory, as it is not possible to upload non-survival files from within the Admin pages.");

			if (Category == CustomLeaderboardCategory.TimeAttack && spawnset.GameMode != GameMode.TimeAttack
			 || Category != CustomLeaderboardCategory.TimeAttack && spawnset.GameMode == GameMode.TimeAttack)
			{
				modelState.AddModelError($"AdminDto.{nameof(Category)}", $"Spawnset game mode is {spawnset.GameMode} while custom leaderboard category is {Category}.");
				return false;
			}

			return true;

			string? GetInvalidDaggerTime()
			{
				if (Category.IsAscending())
				{
					if (TimeLeviathan >= TimeDevil)
						return nameof(TimeLeviathan);
					if (TimeDevil >= TimeGolden)
						return nameof(TimeDevil);
					if (TimeGolden >= TimeSilver)
						return nameof(TimeGolden);
					if (TimeSilver >= TimeBronze)
						return nameof(TimeSilver);
				}
				else
				{
					if (TimeLeviathan <= TimeDevil)
						return nameof(TimeLeviathan);
					if (TimeDevil <= TimeGolden)
						return nameof(TimeDevil);
					if (TimeGolden <= TimeSilver)
						return nameof(TimeGolden);
					if (TimeSilver <= TimeBronze)
						return nameof(TimeSilver);
				}

				return null;
			}
		}
	}
}
