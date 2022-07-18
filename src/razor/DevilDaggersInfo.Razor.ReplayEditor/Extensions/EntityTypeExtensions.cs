using DevilDaggersInfo.Core.Replay.Enums;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Structs;

namespace DevilDaggersInfo.Razor.ReplayEditor.Extensions;

public static class EntityTypeExtensions
{
	public static Color GetColor(this EntityType entityType) => entityType switch
	{
		EntityType.Level1Dagger => UpgradeColors.Level1,
		EntityType.Level2Dagger => UpgradeColors.Level2,
		EntityType.Level3Dagger => UpgradeColors.Level3,
		EntityType.Level3HomingDagger => UpgradeColors.Level3, // TODO: Different colors.
		EntityType.Level4Dagger => UpgradeColors.Level4,
		EntityType.Level4HomingDagger => UpgradeColors.Level4, // TODO: Different colors.
		EntityType.Level4HomingSplash => UpgradeColors.Level4, // TODO: Different colors.
		EntityType.Squid1 => EnemyColors.Squid1,
		EntityType.Squid2 => EnemyColors.Squid2,
		EntityType.Squid3 => EnemyColors.Squid3,
		EntityType.Skull1 => EnemyColors.Skull1,
		EntityType.Skull2 => EnemyColors.Skull2,
		EntityType.Skull3 => EnemyColors.Skull3,
		EntityType.Spiderling => EnemyColors.Spiderling,
		EntityType.Skull4 => EnemyColors.Skull4,
		EntityType.Centipede => EnemyColors.Centipede,
		EntityType.Gigapede => EnemyColors.Gigapede,
		EntityType.Ghostpede => EnemyColors.Ghostpede,
		EntityType.Spider1 => EnemyColors.Spider1,
		EntityType.Spider2 => EnemyColors.Spider2,
		EntityType.SpiderEgg => EnemyColors.SpiderEgg1,
		EntityType.Leviathan => EnemyColors.Leviathan,
		EntityType.Thorn => EnemyColors.Thorn,
		EntityType.Player => new(64, 64, 255),
		_ => new(255, 255, 255),
	};

	public static string ToDisplayString(this EntityType entityType) => entityType switch
	{
		EntityType.Level1Dagger => "Level 1 dagger",
		EntityType.Level2Dagger => "Level 2 dagger",
		EntityType.Level3Dagger => "Level 3 dagger",
		EntityType.Level3HomingDagger => "Level 3 homing dagger",
		EntityType.Level4Dagger => "Level 4 dagger",
		EntityType.Level4HomingDagger => "Level 4 homing dagger",
		EntityType.Level4HomingSplash => "Level 4 homing splash",
		EntityType.Squid1 => "Squid I",
		EntityType.Squid2 => "Squid II",
		EntityType.Squid3 => "Squid III",
		EntityType.Skull1 => "Skull I",
		EntityType.Skull2 => "Skull II",
		EntityType.Skull3 => "Skull III",
		EntityType.Skull4 => "Skull IV",
		EntityType.Spider1 => "Spider I",
		EntityType.Spider2 => "Spider II",
		EntityType.SpiderEgg => "Spider Egg",
		_ => entityType.ToString(),
	};
}
