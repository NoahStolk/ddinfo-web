using DevilDaggersInfo.Api.Main.Players;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Converters;

public static class PlayerConverters
{
	public static VerticalSync ToMainApi(this Types.Web.VerticalSync verticalSync) => verticalSync switch
	{
		Types.Web.VerticalSync.Unknown => VerticalSync.Unknown,
		Types.Web.VerticalSync.Off => VerticalSync.Off,
		Types.Web.VerticalSync.On => VerticalSync.On,
		Types.Web.VerticalSync.Adaptive => VerticalSync.Adaptive,
		_ => throw new UnreachableException(),
	};

	public static Types.Web.VerticalSync ToDomain(this VerticalSync verticalSync) => verticalSync switch
	{
		VerticalSync.Unknown => Types.Web.VerticalSync.Unknown,
		VerticalSync.Off => Types.Web.VerticalSync.Off,
		VerticalSync.On => Types.Web.VerticalSync.On,
		VerticalSync.Adaptive => Types.Web.VerticalSync.Adaptive,
		_ => throw new UnreachableException(),
	};
}
