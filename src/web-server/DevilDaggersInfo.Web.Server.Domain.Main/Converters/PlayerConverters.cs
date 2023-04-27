using DevilDaggersInfo.Api.Main.Players;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Converters;

public static class PlayerConverters
{
	public static VerticalSync ToMainApi(this Entities.Enums.VerticalSync verticalSync) => verticalSync switch
	{
		Entities.Enums.VerticalSync.Unknown => VerticalSync.Unknown,
		Entities.Enums.VerticalSync.Off => VerticalSync.Off,
		Entities.Enums.VerticalSync.On => VerticalSync.On,
		Entities.Enums.VerticalSync.Adaptive => VerticalSync.Adaptive,
		_ => throw new UnreachableException(),
	};

	public static Entities.Enums.VerticalSync ToDomain(this VerticalSync verticalSync) => verticalSync switch
	{
		VerticalSync.Unknown => Entities.Enums.VerticalSync.Unknown,
		VerticalSync.Off => Entities.Enums.VerticalSync.Off,
		VerticalSync.On => Entities.Enums.VerticalSync.On,
		VerticalSync.Adaptive => Entities.Enums.VerticalSync.Adaptive,
		_ => throw new UnreachableException(),
	};
}
