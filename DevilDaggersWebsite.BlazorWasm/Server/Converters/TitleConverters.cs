using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Titles;
using DevilDaggersWebsite.Entities;

namespace DevilDaggersWebsite.BlazorWasm.Server.Converters
{
	public static class TitleConverters
	{
		public static GetTitle ToGetTitle(this Title title) => new()
		{
			Id = title.Id,
			Name = title.Name,
			PlayerIds = title.PlayerTitles.ConvertAll(pt => pt.PlayerId),
		};
	}
}
