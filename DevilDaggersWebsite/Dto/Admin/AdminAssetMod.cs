using DevilDaggersWebsite.Enumerators;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminAssetMod : IAdminDto
	{
		public List<int>? PlayerIds { get; init; }
		public List<AssetModTypes>? AssetModTypes { get; init; }
		public string Name { get; init; } = null!;
		public string? Url { get; init; }
		public bool IsHidden { get; set; }
		public DateTime LastUpdated { get; set; }
		public string? TrailerUrl { get; set; }
		public string? HtmlDescription { get; set; }

		public Dictionary<string, string> Log()
		{
			Dictionary<string, string> dictionary = new();
			dictionary.Add(nameof(PlayerIds), PlayerIds != null ? string.Join(", ", PlayerIds) : string.Empty);
			dictionary.Add(nameof(AssetModTypes), AssetModTypes != null ? string.Join(", ", AssetModTypes.Where(amt => amt != Enumerators.AssetModTypes.None)) : string.Empty);
			dictionary.Add(nameof(Name), Name);
			dictionary.Add(nameof(Url), Url ?? string.Empty);
			dictionary.Add(nameof(IsHidden), IsHidden.ToString());
			dictionary.Add(nameof(LastUpdated), LastUpdated.ToString());
			dictionary.Add(nameof(TrailerUrl), TrailerUrl ?? string.Empty);
			dictionary.Add(nameof(HtmlDescription), HtmlDescription ?? string.Empty);
			return dictionary;
		}

		public bool ValidateGlobal(ModelStateDictionary modelState)
		{
			if (PlayerIds == null || PlayerIds.Count == 0)
			{
				modelState.AddModelError($"AdminDto.{nameof(PlayerIds)}", "Mod should have at least one author.");
				return false;
			}

			return true;
		}
	}
}
