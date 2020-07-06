using Newtonsoft.Json;

namespace DevilDaggersWebsite.Code.Users
{
	public class PlayerSetting : AbstractUserData
	{
		public override string FileName => "settings";

		public int Id { get; set; }
		public int? Dpi { get; set; }
		public float? InGameSens { get; set; }
		public int? Fov { get; set; }
		public bool? RightHanded { get; set; }
		public bool? FlashEnabled { get; set; }
		public float? Gamma { get; set; }

		[JsonIgnore]
		public float? Edpi => Dpi * InGameSens;
		[JsonIgnore]
		public string RightHandedString => !RightHanded.HasValue ? string.Empty : RightHanded.Value ? "Right" : "Left";
		[JsonIgnore]
		public string FlashEnabledString => !FlashEnabled.HasValue ? string.Empty : FlashEnabled.Value ? "On" : "Off";
	}
}