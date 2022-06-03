using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Web.Server.Entities.Enums;
using MainApi = DevilDaggersInfo.Api.Main.Players;

namespace DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Main;

public static class PlayerConverters
{
	public static VerticalSync ToDomain(this MainApi.VerticalSync verticalSync) => verticalSync switch
	{
		MainApi.VerticalSync.Unknown => VerticalSync.Unknown,
		MainApi.VerticalSync.Off => VerticalSync.Off,
		MainApi.VerticalSync.On => VerticalSync.On,
		MainApi.VerticalSync.Adaptive => VerticalSync.Adaptive,
		_ => throw new InvalidEnumConversionException(verticalSync),
	};
}
