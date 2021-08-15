using DevilDaggersInfo.Web.BlazorWasm.Server.Entities;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Titles;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Admin;

public static class TitleConverters
{
	public static GetTitle ToGetTitle(this TitleEntity title) => new()
	{
		Id = title.Id,
		Name = title.Name,
	};
}
