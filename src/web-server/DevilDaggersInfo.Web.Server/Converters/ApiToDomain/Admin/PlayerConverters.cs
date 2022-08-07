using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using AdminApi = DevilDaggersInfo.Api.Admin.Players;

namespace DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Admin;

public static class PlayerConverters
{
	public static VerticalSync ToDomain(this AdminApi.VerticalSync verticalSync) => verticalSync switch
	{
		AdminApi.VerticalSync.Unknown => VerticalSync.Unknown,
		AdminApi.VerticalSync.Off => VerticalSync.Off,
		AdminApi.VerticalSync.On => VerticalSync.On,
		AdminApi.VerticalSync.Adaptive => VerticalSync.Adaptive,
		_ => throw new InvalidEnumConversionException(verticalSync),
	};

	public static BanType ToDomain(this AdminApi.BanType banType) => banType switch
	{
		AdminApi.BanType.NotBanned => BanType.NotBanned,
		AdminApi.BanType.Alt => BanType.Alt,
		AdminApi.BanType.Cheater => BanType.Cheater,
		AdminApi.BanType.Boosted => BanType.Boosted,
		AdminApi.BanType.IllegitimateStats => BanType.IllegitimateStats,
		AdminApi.BanType.BlankName => BanType.BlankName,
		_ => throw new InvalidEnumConversionException(banType),
	};
}
