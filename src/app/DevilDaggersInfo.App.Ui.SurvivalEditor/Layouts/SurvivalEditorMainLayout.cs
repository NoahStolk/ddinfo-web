using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;
using StateManager = DevilDaggersInfo.App.Ui.Base.StateManagement.StateManager;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditorMainLayout : Layout, IExtendedLayout
{
	private readonly KeySubmitter _keySubmitter = new();

	public SurvivalEditorMainLayout()
	{
		SpawnsWrapper spawnsWrapper = new(new PixelBounds(0, 24, 400, 640));
		ArenaWrapper arenaWrapper = new(new PixelBounds(400, 24, 400, 400));
		SpawnEditor spawnEditor = new(new PixelBounds(0, 664, 384, 128));
		HistoryScrollArea historyScrollArea = new(new PixelBounds(768, 512, 256, 256));
		SettingsWrapper settingsWrapper = new(new PixelBounds(804, 24, 216, 256));

		NestingContext.Add(spawnsWrapper);
		NestingContext.Add(arenaWrapper);
		NestingContext.Add(spawnEditor);
		NestingContext.Add(historyScrollArea);
		NestingContext.Add(settingsWrapper);
	}
}
