using DevilDaggersInfo.Web.ApiSpec.Main.Players;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Converters.DomainToApi;

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
}
