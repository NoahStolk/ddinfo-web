using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.RewriteRules;

/// <summary>
/// Redirects old mod pages from the V4 website. Only the mods that existed at that point are included here (dumped with SQL).
/// </summary>
public class ModPageRewriteRules : IRule
{
	private static readonly Dictionary<int, string> _names = new()
	{
		{ 1, "Xamide's Soundpack" },
		{ 3, "Devil Nails" },
		{ 4, "There is no Spoon" },
		{ 5, "Devil Dongers" },
		{ 6, "Blood of the Fang" },
		{ 7, "Epic True Soundpack v2" },
		{ 8, "Epic True Soundpack" },
		{ 9, "FUNNYSOUNDS" },
		{ 10, "Perfect Demons" },
		{ 12, "Nifty Daggers" },
		{ 13, "Braden's Gfxpacks" },
		{ 14, "Braden's Soundpacks" },
		{ 15, "Deathgrips" },
		{ 17, "Half-Life 1 Sounds" },
		{ 18, "Newports (audio)" },
		{ 20, "post_lut Experiments" },
		{ 21, "Nulls Cancerpack" },
		{ 22, "Emerald Daggers" },
		{ 23, "Milky Daggers" },
		{ 24, "The Definitive Experience" },
		{ 25, "MCDD (Minecraft)" },
		{ 26, "Magic Missilez" },
		{ 27, "Added Sugar" },
		{ 28, "dabSkull" },
		{ 29, "Magic Bulletz" },
		{ 30, "Eggil Eggers" },
		{ 31, "Hell Tastes Nasty" },
		{ 32, "Hell is Cold" },
		{ 33, "Menu Music" },
		{ 34, "Pritster's Earbleeder v2" },
		{ 35, "Obtain" },
		{ 36, "Cake's Soundpack" },
		{ 37, "Deathgrips 2.0" },
		{ 38, "Color out of Hell" },
		{ 39, "EVERYTHING IS ME" },
		{ 40, "Underline" },
		{ 42, "Neon Knights" },
		{ 43, "Jojo's Bizarre Adventure" },
		{ 44, "Shadows Die Thousand" },
		{ 46, "ShootingStarz" },
		{ 47, "Epic True Soundpack v3" },
		{ 49, "Grape Daggers" },
		{ 50, "Magic Arrowz" },
		{ 51, "Supervixen" },
		{ 52, "ÆTHEREA" },
		{ 53, "HealerGirl Mods" },
		{ 54, "Spidorlingu Dark" },
		{ 55, "Spidorlingu" },
		{ 56, "Muted death sound" },
		{ 57, "Linguchad" },
		{ 58, "meme megapack v1.0" },
		{ 59, "Blood Runs" },
		{ 61, "Added Spice" },
		{ 62, "Custom Mix" },
		{ 63, "Custom Mix II" },
		{ 64, "V2 Gigapedes" },
		{ 65, "Dither Daggers" },
		{ 67, "Bottomless Plates" },
		{ 68, "Newports" },
		{ 69, "Axe's Untitled Soundpack" },
		{ 70, "LocoCaesar's mini mods" },
		{ 71, "LocoCaesar's post_luts" },
		{ 73, "Cursed Cutters" },
		{ 75, "Frostbite" },
		{ 76, "ddddd" },
		{ 77, "Transmutation" },
		{ 78, "Dryland" },
		{ 79, "ddquid" },
		{ 80, "Manspaghetti Splash Music" },
		{ 81, "ddababy" },
		{ 82, "Blueshift" },
		{ 83, "Heart Monitor Splash Screen" },
		{ 84, "Blueshift Ripoffs" },
		{ 87, "Ambient Occlusion" },
		{ 88, "Ice" },
		{ 89, "lossofbraincells" },
		{ 90, "TheThingThatShouldNotBeDeathSFX" },
		{ 91, "Grayscale" },
		{ 92, "Classic lvl3 flash" },
		{ 93, "Black Rust" },
		{ 94, "Alternative Dagger Colors" },
		{ 95, "STRAFE" },
		{ 96, "DUSK" },
		{ 97, "Rainbow Pack" },
		{ 98, "Skybox" },
		{ 99, "RedFiles" },
		{ 100, "Among Us" },
		{ 101, "ARCADE" },
		{ 102, "dd1x1" },
		{ 104, "Hand Skybox" },
		{ 105, "Grey Light" },
		{ 106, "dd_advanced" },
		{ 107, "PurpleDDPink" },
		{ 108, "Tutor" },
		{ 109, "handbox_azad_model" },
		{ 110, "Tall Tiles" },
		{ 111, "handbox_zigi" },
		{ 113, "Delta Gems" },
		{ 114, "No Hand" },
		{ 115, "Zebra" },
		{ 116, "Bioluminescence" },
		{ 117, "Gucci" },
		{ 118, "Level 4 Hands Collection" },
		{ 119, "rgb" },
		{ 120, "Gem Health" },
		{ 121, "GitGood" },
		{ 122, "axetroll" },
		{ 123, "kromer" },
		{ 124, "Nitri Hand" },
		{ 125, "Ambient Outline" },
		{ 126, "Ethereal" },
		{ 127, "Rune Gem Pack" },
		{ 128, "akagumo" },
		{ 129, "midorimukade" },
		{ 130, "Tkmiz Tiles" },
		{ 131, "Rainbowflash" },
		{ 132, "gigathorn" },
		{ 133, "Pale_Pedes" },
	};

	private static readonly Dictionary<int, string> _escapedNames = _names.ToDictionary(kvp => kvp.Key, kvp => Uri.EscapeDataString(kvp.Value));

	public void ApplyRule(RewriteContext context)
	{
		HttpRequest request = context.HttpContext.Request;
		if (request.Path.Value == null || request.QueryString.Value == null || RewriteRuleUtils.EndsWithContent(request.Path.Value))
			return;

		if (!request.Path.Value.Equals("/Mod", StringComparison.OrdinalIgnoreCase))
			return;

		const string queryStringKey = "?mod=";
		if (!request.QueryString.Value.StartsWith(queryStringKey, StringComparison.OrdinalIgnoreCase))
			return;

		string modName = request.QueryString.Value.TrimStart(queryStringKey);
		int? modId = _escapedNames.ContainsValue(modName) ? _escapedNames.First(kvp => kvp.Value == modName).Key : null;
		if (!modId.HasValue)
			return;

		context.Result = RuleResult.EndResponse;

		context.HttpContext.Response.StatusCode = StatusCodes.Status301MovedPermanently;
		context.HttpContext.Response.Headers[HeaderNames.Location] = $"{request.Scheme}://{request.Host}/custom/mod/{modId.Value}";
	}
}
