using DevilDaggersInfo.Types.Core.Replays;

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

	public static int GetInitialTransmuteHp(this EntityType entityType) => entityType switch
	{
		EntityType.Skull1 => 10,
		EntityType.Skull2 => 20,
		EntityType.Skull3 => 100,
		EntityType.Skull4 => 300,
		EntityType.Leviathan => 2400,
		_ => throw new InvalidOperationException($"{nameof(EntityType)} '{entityType}' cannot be transmuted."),
	};

	// TODO: Take enemy parts (UserData) into account? Some parts being damaged does 0 damage, like spider/squid body, or empty pede segment.
	public static int GetDamage(this EntityType enemyType, EntityType daggerType)
	{
		if (!enemyType.IsEnemy())
			throw new InvalidOperationException($"Type '{enemyType}' must be an enemy.");

		if (!daggerType.IsDagger())
			throw new InvalidOperationException($"Type '{daggerType}' must be a dagger.");

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
				_ => 0,
			},
			_ => 0,
		};
	}
}
