using CoreBase.Code;
using DevilDaggersWebsite.Pages;

namespace DevilDaggersWebsite.Models.Spawnset
{
	public class SpawnsetNavigationSettings
	{
		public SpawnsetsModel Model { get; set; }
		public int MaxAround { get; set; }
		public ScreenWidthVisibility Visibility { get; set; }

		public SpawnsetNavigationSettings(SpawnsetsModel model, int maxAround, ScreenWidthVisibility visibility)
		{
			Model = model;
			MaxAround = maxAround;
			Visibility = visibility;
		}
	}
}