using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.RewriteRules;

/// <summary>
/// Redirects old custom leaderboard pages from the V4 website. Only the custom leaderboards that existed at that point are included here (dumped with SQL).
/// </summary>
public class CustomLeaderboardPageRewriteRules : IRule
{
	private static readonly Dictionary<int, string> _names = new()
	{
		{ 1, "AttackAttackAttack" },
		{ 3, "bintrs cool spawnset" },
		{ 4, "HellOnSteroids" },
		{ 5, "TheFourLeviathanOfTheApocalypse" },
		{ 6, "Aim_Training" },
		{ 9, "FourDigitSkullSplitter" },
		{ 10, "Coil" },
		{ 12, "bigplate" },
		{ 13, "Panic Stations" },
		{ 14, "Beyblade" },
		{ 15, "V1" },
		{ 16, "V2" },
		{ 17, "infinity_farm" },
		{ 20, "Intimidate" },
		{ 21, "Smoke" },
		{ 22, "Kill" },
		{ 23, "PrettyHard" },
		{ 25, "littleplate" },
		{ 26, "The Playground" },
		{ 27, "Braniac (You Suck At Dagger Jumps)" },
		{ 28, "succ" },
		{ 29, "Braniac - For Smooth Brains" },
		{ 31, "AttackAttackAttack2" },
		{ 32, "BradensNiftySpawnset" },
		{ 34, "nanospawn" },
		{ 35, "V3 but Levi is at the start" },
		{ 36, "TooManySkulls" },
		{ 38, "FluidMovment" },
		{ 39, "Loopy" },
		{ 40, "Bonk" },
		{ 42, "hell_wants_you_back" },
		{ 44, "abyss" },
		{ 46, "dont_die_the_stretch_V2" },
		{ 47, "Ghostbusters" },
		{ 48, "annoys_me_to_no_end" },
		{ 50, "Sushi" },
		{ 52, "FastestCentipedeKill" },
		{ 53, "FastestSquidIIIKill" },
		{ 54, "FastestSquidIIKill" },
		{ 55, "FastestSquidIKill" },
		{ 56, "FastestSpiderIKill" },
		{ 57, "FastestThornKill" },
		{ 58, "FastestGigapedeKill" },
		{ 59, "Endure" },
		{ 60, "Transmuted4" },
		{ 61, "donut_die" },
		{ 62, "TooMuchSushi" },
		{ 63, "300SecondsOfHell" },
		{ 64, "V3 1.33x faster" },
		{ 65, "V3 10x faster" },
		{ 66, "Bradens_Gauntlet" },
		{ 69, "ikayaki" },
		{ 70, "Volatile" },
		{ 71, "Inverse Loop" },
		{ 72, "v1.33x_LOOP_ONL" },
		{ 73, "HowFastCanYouGo" },
		{ 74, "TransmutedBonk" },
		{ 75, "DefendDefendDefend" },
		{ 76, "DefendDefendDefend2" },
		{ 77, "V0" },
		{ 78, "Scanner" },
		{ 79, "TinySpeedrun1" },
		{ 80, "TinySpeedrun2" },
		{ 81, "crumblecade" },
		{ 83, "AttackAttackAttackCentipede" },
		{ 84, "hellish_loop" },
		{ 85, "exopalace" },
		{ 86, "KingdomOfSpiders2" },
		{ 87, "YouMustFearTheDark2" },
		{ 88, "No Homo Leviathan" },
		{ 89, "V3-but-youre-stuck-with-level-1" },
		{ 91, "V3-but-youre-stuck-with-level-3" },
		{ 93, "TinySpeedrun3" },
		{ 94, "TinySpeedrun2ButTheWallsAreHigher" },
		{ 95, "SmallSpeedrunButItsReallyBad" },
		{ 96, "TinyJumps1" },
		{ 97, "No Homo Giga Trio Level 3" },
		{ 98, "deathway" },
		{ 99, "jetway" },
		{ 100, "V3 doubled (balanced)" },
		{ 101, "Thorns and a side of Pedes" },
		{ 102, "HowFastCanYouGoCenti" },
		{ 103, "powercase" },
		{ 104, "kikenthar" },
		{ 105, "SquidHellPrac" },
		{ 106, "PlatinumPlatPlatener" },
		{ 107, "Speeeeeeeed" },
		{ 108, "Warm_up" },
		{ 110, "HardCore" },
		{ 111, "HardCore_lite" },
		{ 112, "Oh lord They comin" },
		{ 113, "SquidHell" },
		{ 114, "SpeedV2" },
		{ 115, "Limbo" },
		{ 117, "Lust" },
		{ 118, "Gluttony" },
		{ 119, "Greed" },
		{ 120, "mokuton" },
		{ 121, "Pedeslayer" },
		{ 122, "skyway" },
		{ 123, "up" },
		{ 124, "noodle_soup" },
		{ 125, "AngerOld" },
		{ 126, "exostate" },
		{ 127, "Heresy" },
		{ 128, "Anger" },
		{ 129, "chappie" },
		{ 130, "CrosshairMaze" },
		{ 131, "confetti" },
		{ 133, "Jump Knight" },
		{ 134, "Dont_Fall" },
		{ 135, "Fast_loop" },
		{ 136, "No Homo 680" },
		{ 137, "yolandi v2" },
		{ 138, "yolandi v1" },
		{ 139, "TinyTowerOfShit" },
	};

	private static readonly Dictionary<int, string> _escapedNames = _names.ToDictionary(kvp => kvp.Key, kvp => Uri.EscapeDataString(kvp.Value));

	public void ApplyRule(RewriteContext context)
	{
		HttpRequest request = context.HttpContext.Request;
		if (request.Path.Value == null || request.QueryString.Value == null || RewriteRuleUtils.EndsWithContent(request.Path.Value))
			return;

		if (!request.Path.Value.Equals("/CustomLeaderboards/Leaderboard", StringComparison.OrdinalIgnoreCase))
			return;

		const string queryStringKey = "?spawnsetName=";
		if (!request.QueryString.Value.StartsWith(queryStringKey, StringComparison.OrdinalIgnoreCase))
			return;

		string spawnsetName = request.QueryString.Value.TrimStart(queryStringKey);
		int? customLeaderboardId = _escapedNames.ContainsValue(spawnsetName) ? _escapedNames.First(kvp => kvp.Value == spawnsetName).Key : null;
		if (!customLeaderboardId.HasValue)
			return;

		context.Result = RuleResult.EndResponse;

		context.HttpContext.Response.StatusCode = StatusCodes.Status301MovedPermanently;
		context.HttpContext.Response.Headers[HeaderNames.Location] = $"{request.Scheme}://{request.Host}/custom/leaderboard/{customLeaderboardId.Value}";
	}
}
