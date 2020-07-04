using Newtonsoft.Json;

namespace DevilDaggersWebsite.Code.Users
{
	public class PlayerSetting
	{
		public int Id { get; set; }
		public int? Dpi { get; set; }
		public float? InGameSens { get; set; }
		public int? Fov { get; set; }
		public bool? RightHanded { get; set; }
		public bool? FlashEnabled { get; set; }

		[JsonIgnore]
		public float? Edpi => Dpi * InGameSens;
		[JsonIgnore]
		public string RightHandedString => !RightHanded.HasValue ? string.Empty : RightHanded.Value ? "Right" : "Left";
		[JsonIgnore]
		public string FlashEnabledString => !FlashEnabled.HasValue ? string.Empty : FlashEnabled.Value ? "On" : "Off";

		public PlayerSetting(int id, int? dpi, float? inGameSens, int? fov, bool? rightHanded, bool? flashEnabled)
		{
			Id = id;
			Dpi = dpi;
			InGameSens = inGameSens;
			Fov = fov;
			RightHanded = rightHanded;
			FlashEnabled = flashEnabled;
		}
	}
}