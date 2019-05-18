namespace DevilDaggersWebsite.Models.Users
{
	public class PlayerSetting
	{
		public int ID { get; set; }
		public int? DPI { get; set; }
		public float? InGameSens { get; set; }
		public int? FOV { get; set; }
		public bool? RightHanded { get; set; }
		public bool? FlashEnabled { get; set; }

		public float? EDPI => DPI * InGameSens;
		public string RightHandedString => !RightHanded.HasValue ? string.Empty : RightHanded.Value ? "Right" : "Left";
		public string FlashEnabledString => !FlashEnabled.HasValue ? string.Empty : FlashEnabled.Value ? "On" : "Off";

		public PlayerSetting(int id, int? dpi, float? inGameSens, int? fov, bool? rightHanded, bool? flashEnabled)
		{
			ID = id;
			DPI = dpi;
			InGameSens = inGameSens;
			FOV = fov;
			RightHanded = rightHanded;
			FlashEnabled = flashEnabled;
		}

		public PlayerSetting(int id)
		{
			ID = id;
			DPI = null;
			InGameSens = null;
			FOV = null;
			RightHanded = null;
			FlashEnabled = null;
		}
	}
}