namespace DevilDaggersInfo.Web.Client.Extensions;

public static class EnemyExtensions
{
	public static string GetImageName(this Enemy enemy)
	{
		if (enemy == EnemiesV1_0.Gigapede || enemy == EnemiesV2_0.Gigapede)
			return "gigapede-red";

		return enemy.Name
			.Replace(" IV", "-4")
			.Replace(" III", "-3")
			.Replace(" II", "-2")
			.Replace(" I", "-1")
			.Replace(' ', '-')
			.ToLower();
	}

	public static string GetUrl(this Enemy enemy)
	{
		string enemyName = enemy.Name.ToLower();
		if (enemyName.Contains("skull"))
			return "skull";

		if (enemyName.StartsWith("squid"))
			return "squid";

		if (enemyName.EndsWith("orb"))
			return "leviathan";

		return enemyName.StartsWith("spider") ? "spider" : enemyName;
	}
}
