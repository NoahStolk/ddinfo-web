using DevilDaggersInfo.Web.ApiSpec.Admin.Players;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters.ApiToDomain;

public static class PlayerConverters
{
	public static Entities.Enums.BanType ToDomain(this BanType banType) => banType switch
	{
		BanType.NotBanned => Entities.Enums.BanType.NotBanned,
		BanType.Alt => Entities.Enums.BanType.Alt,
		BanType.Cheater => Entities.Enums.BanType.Cheater,
		BanType.Boosted => Entities.Enums.BanType.Boosted,
		BanType.IllegitimateStats => Entities.Enums.BanType.IllegitimateStats,
		BanType.BlankName => Entities.Enums.BanType.BlankName,
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
