using DevilDaggersWebsite.Razor.PageModels;

namespace DevilDaggersWebsite.Razor.Pagination
{
	public class NavigationSettings
	{
		public NavigationSettings(IPaginationModel model, int maxAround, ScreenWidthVisibilities visibility)
		{
			Model = model;
			MaxAround = maxAround;
			Visibility = visibility;
		}

		public IPaginationModel Model { get; set; }
		public int MaxAround { get; set; }
		public ScreenWidthVisibilities Visibility { get; set; }
	}
}
