using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.Titles;

namespace DevilDaggersWebsite.BlazorWasm.Server.Converters.Admin
{
	public static class TitleConverters
	{
		public static GetTitle ToGetTitle(this Title title) => new()
		{
			Id = title.Id,
			Name = title.Name,
		};
	}
}
