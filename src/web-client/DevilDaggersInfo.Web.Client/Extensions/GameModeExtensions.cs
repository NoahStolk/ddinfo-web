using DevilDaggersInfo.Api.Main.Spawnsets;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class GameModeExtensions
{
	public static string GetDescription(this GameMode category) => category switch
	{
		GameMode.Survival => "Survive as long as you can.",
		GameMode.TimeAttack => "Kill all enemies as quickly as possible.",
		GameMode.Race => "Reach the dagger as quickly as possible.",
		_ => throw new UnreachableException(),
	};
}
