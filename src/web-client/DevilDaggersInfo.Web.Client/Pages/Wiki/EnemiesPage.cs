using DevilDaggersInfo.Types.Core.Wiki;

namespace DevilDaggersInfo.Web.Client.Pages.Wiki;

public partial class EnemiesPage
{
	private readonly IReadOnlyList<GameVersion> _allowedGameVersions = new List<GameVersion> { GameVersion.V1_0, GameVersion.V2_0, GameVersion.V3_0, GameVersion.V3_1, GameVersion.V3_2 };

	public GameVersion GameVersion { get; set; } = GameConstants.CurrentVersion;

	private void UpdateGameVersion(GameVersion gameVersion)
	{
		GameVersion = gameVersion;
	}
}
