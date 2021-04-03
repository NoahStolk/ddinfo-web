using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Razor.Pages
{
	public class IndexModel : PageModel
	{
		public List<(string Text, string ImageUrl, string Url)> Sections { get; } = new()
		{
			{ ("View the official leaderboard", "Home/Leaderboard.png", "/Leaderboard") },
			{ ("Check out mods made by the community", "Home/Mod.png", "/Mods") },
			{ ("Check out spawnsets made by the community", "Home/Spawnset.png", "/Spawnsets") },
			{ ("Check out the asset editor which can be used to mod the game", "Tools/DevilDaggersAssetEditor.png", "/Tools/DevilDaggersAssetEditor") },
			{ ("Check out the survival editor which can be used to create spawnsets and practice sections of the game", "Tools/DevilDaggersSurvivalEditor.png", "/Tools/DevilDaggersSurvivalEditor") },
			{ ("Participate in custom leaderboards for spawnsets", "Home/CustomLeaderboard.png", "/CustomLeaderboards") },
			{ ("Check out the world record progression or other statistics", "Home/Statistics.png", "/Leaderboard/WorldRecordProgression") },
			{ ("Check out various wiki pages about the game, such as spawn times, enemy types, or hand upgrades", "Home/Wiki.png", "/Wiki/Enemies") },
		};
	}
}
