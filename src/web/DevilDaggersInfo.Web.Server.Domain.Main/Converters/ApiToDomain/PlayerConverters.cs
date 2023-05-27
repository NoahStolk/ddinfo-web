using DevilDaggersInfo.Api.Main.Players;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Converters.ApiToDomain;

public static class PlayerConverters
{
	public static Entities.Enums.VerticalSync ToDomain(this VerticalSync verticalSync) => verticalSync switch
	{
		VerticalSync.Unknown => Entities.Enums.VerticalSync.Unknown,
		VerticalSync.Off => Entities.Enums.VerticalSync.Off,
		VerticalSync.On => Entities.Enums.VerticalSync.On,
		VerticalSync.Adaptive => Entities.Enums.VerticalSync.Adaptive,
		_ => throw new UnreachableException(),
	};
}
