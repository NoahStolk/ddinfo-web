using System.Collections.Generic;

namespace DevilDaggersWebsite.Entities
{
	public class Player
	{
		public int Id { get; set; }

		public string Username { get; set; }
		public bool IsAnonymous { get; set; }
		public List<PlayerAssetMod> PlayerAssetMods { get; set; }
		public List<PlayerTitle> PlayerTitles { get; set; }
		public string? CountryCode { get; set; }
		public int? Dpi { get; set; }
		public float? InGameSens { get; set; }
		public int? Fov { get; set; }
		public bool? RightHanded { get; set; }
		public bool? FlashEnabled { get; set; }
		public float? Gamma { get; set; }
		public bool IsBanned { get; set; }
		public string? BanDescription { get; set; }
		public int? BanResponsibleId { get; set; }

		public float? Edpi => Dpi * InGameSens;
		public string RightHandedString => !RightHanded.HasValue ? string.Empty : RightHanded.Value ? "Right" : "Left";
		public string FlashEnabledString => !FlashEnabled.HasValue ? string.Empty : FlashEnabled.Value ? "On" : "Off";

		public override string ToString()
			=> $"{Username} ({Id})";
	}
}
