using DevilDaggersInfo.Core.Replay.Events.Enums;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Structs;

namespace DevilDaggersInfo.Core.Replay.Extensions;

public static class EntityTypeExtensions
{
	public static bool IsEnemy(this EntityType entityType)
		=> !entityType.IsDagger() && entityType != EntityType.Zero;

	public static bool IsDagger(this EntityType entityType)
		=> entityType is EntityType.Level1Dagger or EntityType.Level2Dagger or EntityType.Level3Dagger or EntityType.Level3HomingDagger or EntityType.Level4Dagger or EntityType.Level4HomingDagger or EntityType.Level4HomingSplash;

	public static DaggerType GetDaggerType(this EntityType entityType) => entityType switch
	{
		EntityType.Level1Dagger => DaggerType.Level1,
		EntityType.Level2Dagger => DaggerType.Level2,
		EntityType.Level3Dagger => DaggerType.Level3,
		EntityType.Level3HomingDagger => DaggerType.Level3Homing,
		EntityType.Level4Dagger => DaggerType.Level4,
		EntityType.Level4HomingDagger => DaggerType.Level4Homing,
		EntityType.Level4HomingSplash => DaggerType.Level4HomingSplash,
		_ => throw new InvalidOperationException($"{nameof(EntityType)} '{entityType}' is not a dagger."),
	};

	public static int GetInitialHp(this EntityType entityType) => entityType switch
	{
		EntityType.Skull1 => 1,
		EntityType.Skull2 => 5,
		EntityType.Skull3 => 10,
		EntityType.Skull4 => 100,
		EntityType.Squid1 => 10,
		EntityType.Squid2 => 20,
		EntityType.Squid3 => 90,
		EntityType.Centipede => 75,
		EntityType.Gigapede => 250,
		EntityType.Ghostpede => 500,
		EntityType.Leviathan => 1500,
		EntityType.Spider1 => 25,
		EntityType.Spider2 => 200,
		EntityType.SpiderEgg => 3,
		EntityType.Spiderling => 3,
		EntityType.Thorn => 120,
		_ => throw new InvalidOperationException($"{nameof(EntityType)} '{entityType}' is not an enemy."),
	};

	private static bool IsWeakPoint(this EntityType entityType, int userData) => entityType switch
	{
		EntityType.Squid2 => userData is >= 0 and < 2,
		EntityType.Squid3 => userData is >= 0 and < 3,
		EntityType.Leviathan => userData is >= 0 and < 6,
		EntityType.Squid1 or EntityType.Spider1 or EntityType.Spider2 => userData == 0,
		_ => true, // Everything else is a hit by default, including pedes (when damaging a dead pede segment, the ID is negated, which we ignore anyway.
	};

	public static int GetInitialTransmuteHp(this EntityType entityType) => entityType switch
	{
		EntityType.Skull1 => 10,
		EntityType.Skull2 => 20,
		EntityType.Skull3 => 100,
		EntityType.Skull4 => 300,
		EntityType.Leviathan => 2400,
		_ => throw new InvalidOperationException($"{nameof(EntityType)} '{entityType}' cannot be transmuted."),
	};

	public static int GetDamage(this EntityType enemyType, EntityType daggerType, int userData)
	{
		if (!enemyType.IsEnemy())
			throw new InvalidOperationException($"Type '{enemyType}' must be an enemy.");

		if (!daggerType.IsDagger())
			throw new InvalidOperationException($"Type '{daggerType}' must be a dagger.");

		if (!enemyType.IsWeakPoint(userData))
			return 0;

		return daggerType switch
		{
			EntityType.Level1Dagger or EntityType.Level2Dagger or EntityType.Level3Dagger or EntityType.Level4Dagger => 1,
			EntityType.Level3HomingDagger => enemyType switch
			{
				EntityType.Squid3 => 30, // Probably unintentional
				EntityType.Ghostpede => 0, // Homing phase through Ghostpede
				EntityType.Leviathan => 1, // Homing deals normal damage to Leviathan (and Orb, which is the same enemy in this context)
				_ => 10,
			},
			EntityType.Level4HomingDagger => enemyType switch
			{
				EntityType.Leviathan => 1, // Homing deals normal damage to Leviathan (and Orb, which is the same enemy in this context)
				EntityType.Centipede or EntityType.Gigapede or EntityType.Ghostpede => 0, // Only splash damages pedes (including Ghostpede)
				_ => 10,
			},
			EntityType.Level4HomingSplash => enemyType switch
			{
				EntityType.Thorn => 1, // Thorns are an exception
				EntityType.Centipede or EntityType.Gigapede or EntityType.Ghostpede => 10, // Only splash damages pedes (including Ghostpede)
				_ => 0, // TODO: This is probably wrong. Squids and Skulls should be damaged by splash.
			},
			_ => 0,
		};
	}

	public static Color GetColor(this EntityType? entityType)
	{
		return entityType switch
		{
			EntityType.Level1Dagger => UpgradesV3_2.Level1.Color,
			EntityType.Level2Dagger => UpgradesV3_2.Level2.Color,
			EntityType.Level3Dagger => UpgradesV3_2.Level3.Color, // TODO: Use different color.
			EntityType.Level3HomingDagger => UpgradesV3_2.Level3.Color,
			EntityType.Level4Dagger => UpgradesV3_2.Level4.Color, // TODO: Use different color.
			EntityType.Level4HomingDagger => UpgradesV3_2.Level4.Color,
			EntityType.Level4HomingSplash => UpgradesV3_2.Level4.Color,
			EntityType.Squid1 => EnemiesV3_2.Squid1.Color,
			EntityType.Squid2 => EnemiesV3_2.Squid2.Color,
			EntityType.Squid3 => EnemiesV3_2.Squid3.Color,
			EntityType.Skull1 => EnemiesV3_2.Skull1.Color,
			EntityType.Skull2 => EnemiesV3_2.Skull2.Color,
			EntityType.Skull3 => EnemiesV3_2.Skull3.Color,
			EntityType.Spiderling => EnemiesV3_2.Spiderling.Color,
			EntityType.Skull4 => EnemiesV3_2.Skull4.Color,
			EntityType.Centipede => EnemiesV3_2.Centipede.Color,
			EntityType.Gigapede => EnemiesV3_2.Gigapede.Color,
			EntityType.Ghostpede => EnemiesV3_2.Ghostpede.Color,
			EntityType.Spider1 => EnemiesV3_2.Spider1.Color,
			EntityType.Spider2 => EnemiesV3_2.Spider2.Color,
			EntityType.SpiderEgg => EnemiesV3_2.SpiderEgg1.Color,
			EntityType.Leviathan => EnemiesV3_2.Leviathan.Color,
			EntityType.Thorn => EnemiesV3_2.Thorn.Color,
			_ => new(191, 0, 255),
		};
	}
}
