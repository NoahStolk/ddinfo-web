using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Types.Web;
using Warp.NET.Content;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Extensions;

public static class CustomLeaderboardCriteriaTypeExtensions
{
	public static (Texture Texture, Color Color) GetIcon(this CustomLeaderboardCriteriaType type) => type switch
	{
		CustomLeaderboardCriteriaType.GemsCollected => (WarpTextures.IconGem, Color.Red),
		CustomLeaderboardCriteriaType.GemsDespawned => (WarpTextures.IconGem, Color.Gray(0.3f)),
		CustomLeaderboardCriteriaType.GemsEaten => (WarpTextures.IconGem, Color.Green),
		CustomLeaderboardCriteriaType.EnemiesKilled => (WarpTextures.IconSkull, Color.Orange),
		CustomLeaderboardCriteriaType.DaggersFired => (WarpTextures.IconDagger, Color.Yellow),
		CustomLeaderboardCriteriaType.DaggersHit => (WarpTextures.IconCrosshair, Color.Orange),
		CustomLeaderboardCriteriaType.HomingStored => (WarpTextures.IconHoming, Color.White),
		CustomLeaderboardCriteriaType.HomingEaten => (WarpTextures.IconHomingMask, Color.Red),
		CustomLeaderboardCriteriaType.Skull1Kills => (WarpTextures.IconSkull, EnemiesV3_2.Skull1.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.Skull2Kills => (WarpTextures.IconSkull, EnemiesV3_2.Skull2.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.Skull3Kills => (WarpTextures.IconSkull, EnemiesV3_2.Skull3.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.Skull4Kills => (WarpTextures.IconSkull, EnemiesV3_2.Skull4.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.SpiderlingKills => (WarpTextures.IconSkull, EnemiesV3_2.Spiderling.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.SpiderEggKills => (WarpTextures.IconSkull, EnemiesV3_2.SpiderEgg1.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.Squid1Kills => (WarpTextures.IconSkull, EnemiesV3_2.Squid1.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.Squid2Kills => (WarpTextures.IconSkull, EnemiesV3_2.Squid2.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.Squid3Kills => (WarpTextures.IconSkull, EnemiesV3_2.Squid3.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.CentipedeKills => (WarpTextures.IconSkull, EnemiesV3_2.Centipede.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.GigapedeKills => (WarpTextures.IconSkull, EnemiesV3_2.Gigapede.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.GhostpedeKills => (WarpTextures.IconSkull, EnemiesV3_2.Ghostpede.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.Spider1Kills => (WarpTextures.IconSkull, EnemiesV3_2.Spider1.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.Spider2Kills => (WarpTextures.IconSkull, EnemiesV3_2.Spider2.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.LeviathanKills => (WarpTextures.IconSkull, EnemiesV3_2.Leviathan.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.OrbKills => (WarpTextures.IconSkull, EnemiesV3_2.TheOrb.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.ThornKills => (WarpTextures.IconSkull, EnemiesV3_2.Thorn.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.Skull1sAlive => (WarpTextures.IconSkull, EnemiesV3_2.Skull1.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.Skull2sAlive => (WarpTextures.IconSkull, EnemiesV3_2.Skull2.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.Skull3sAlive => (WarpTextures.IconSkull, EnemiesV3_2.Skull3.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.Skull4sAlive => (WarpTextures.IconSkull, EnemiesV3_2.Skull4.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.SpiderlingsAlive => (WarpTextures.IconSkull, EnemiesV3_2.Spiderling.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.SpiderEggsAlive => (WarpTextures.IconSkull, EnemiesV3_2.SpiderEgg1.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.Squid1sAlive => (WarpTextures.IconSkull, EnemiesV3_2.Squid1.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.Squid2sAlive => (WarpTextures.IconSkull, EnemiesV3_2.Squid2.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.Squid3sAlive => (WarpTextures.IconSkull, EnemiesV3_2.Squid3.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.CentipedesAlive => (WarpTextures.IconSkull, EnemiesV3_2.Centipede.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.GigapedesAlive => (WarpTextures.IconSkull, EnemiesV3_2.Gigapede.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.GhostpedesAlive => (WarpTextures.IconSkull, EnemiesV3_2.Ghostpede.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.Spider1sAlive => (WarpTextures.IconSkull, EnemiesV3_2.Spider1.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.Spider2sAlive => (WarpTextures.IconSkull, EnemiesV3_2.Spider2.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.LeviathansAlive => (WarpTextures.IconSkull, EnemiesV3_2.Leviathan.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.OrbsAlive => (WarpTextures.IconSkull, EnemiesV3_2.TheOrb.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.ThornsAlive => (WarpTextures.IconSkull, EnemiesV3_2.Thorn.Color.ToWarpColor()),
		CustomLeaderboardCriteriaType.DeathType => (WarpTextures.IconSkull, Color.White),
		CustomLeaderboardCriteriaType.Time => (WarpTextures.IconStopwatch, Color.White),
		CustomLeaderboardCriteriaType.LevelUpTime2 => (WarpTextures.IconStopwatch, Color.White),
		CustomLeaderboardCriteriaType.LevelUpTime3 => (WarpTextures.IconStopwatch, Color.White),
		CustomLeaderboardCriteriaType.LevelUpTime4 => (WarpTextures.IconStopwatch, Color.White),
		CustomLeaderboardCriteriaType.EnemiesAlive => (WarpTextures.IconSkull, Color.White),
		_ => (WarpTextures.IconEye, Color.White),
	};
}
