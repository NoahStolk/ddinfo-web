using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminPlayer : IAdminDto
	{
		public int Id { get; init; }
		public string PlayerName { get; init; } = null!;
		public bool IsAnonymous { get; init; }
		public List<int>? AssetModIds { get; init; }
		public List<int>? TitleIds { get; init; }
		public string? CountryCode { get; init; }
		public int? Dpi { get; init; }
		public float? InGameSens { get; init; }
		public int? Fov { get; init; }
		public bool? RightHanded { get; init; }
		public bool? FlashEnabled { get; init; }
		public float? Gamma { get; init; }
		public bool IsBanned { get; init; }
		public string? BanDescription { get; init; }
		public int? BanResponsibleId { get; init; }
		public bool? UsesLegacyAudio { get; init; }
		public bool IsBannedFromDdcl { get; init; }

		public Dictionary<string, string> Log()
		{
			Dictionary<string, string> dictionary = new();
			dictionary.Add(nameof(PlayerName), PlayerName);
			dictionary.Add(nameof(IsAnonymous), IsAnonymous.ToString());
			dictionary.Add(nameof(AssetModIds), AssetModIds != null ? string.Join(", ", AssetModIds) : string.Empty);
			dictionary.Add(nameof(TitleIds), TitleIds != null ? string.Join(", ", TitleIds) : string.Empty);
			dictionary.Add(nameof(CountryCode), CountryCode ?? string.Empty);
			dictionary.Add(nameof(Dpi), Dpi.ToString() ?? string.Empty);
			dictionary.Add(nameof(InGameSens), InGameSens.ToString() ?? string.Empty);
			dictionary.Add(nameof(Fov), Fov.ToString() ?? string.Empty);
			dictionary.Add(nameof(RightHanded), RightHanded.ToString() ?? string.Empty);
			dictionary.Add(nameof(FlashEnabled), FlashEnabled.ToString() ?? string.Empty);
			dictionary.Add(nameof(Gamma), Gamma.ToString() ?? string.Empty);
			dictionary.Add(nameof(IsBanned), IsBanned.ToString());
			dictionary.Add(nameof(BanDescription), BanDescription ?? string.Empty);
			dictionary.Add(nameof(BanResponsibleId), BanResponsibleId.ToString() ?? string.Empty);
			dictionary.Add(nameof(UsesLegacyAudio), UsesLegacyAudio.ToString() ?? string.Empty);
			dictionary.Add(nameof(IsBannedFromDdcl), IsBannedFromDdcl.ToString() ?? string.Empty);
			return dictionary;
		}

		public bool ValidateGlobal(ModelStateDictionary modelState)
		{
			if (IsBanned)
			{
				if (!string.IsNullOrWhiteSpace(CountryCode))
				{
					modelState.AddModelError($"AdminDto.{nameof(CountryCode)}", "Banned players should not have a country code.");
					return false;
				}

				foreach (KeyValuePair<string, bool> kvp in new Dictionary<string, bool>()
				{
					{ nameof(Dpi), Dpi.HasValue },
					{ nameof(InGameSens), InGameSens.HasValue },
					{ nameof(Fov), Fov.HasValue },
					{ nameof(RightHanded), RightHanded.HasValue },
					{ nameof(FlashEnabled), FlashEnabled.HasValue },
					{ nameof(Gamma), Gamma.HasValue },
					{ nameof(UsesLegacyAudio), UsesLegacyAudio.HasValue },
				})
				{
					if (kvp.Value)
					{
						modelState.AddModelError($"AdminDto.{kvp.Key}", "Banned players should not have settings.");
						return false;
					}
				}
			}

			return true;
		}
	}
}
