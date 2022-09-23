using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.SurvivalEditor;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetHistory;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetMenu;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class MainLayout : Layout, ISurvivalEditorMainLayout
{
	public MainLayout()
		: base(new(0, 0, 1920, 1080))
	{
		Menu menu = new(new(0, 0, 1920, 24));
		ArenaWrapper arenaWrapper = new(Rectangle.At(1024, 64, 864, 512));
		SpawnsWrapper = new(Rectangle.At(0, 64, 544, 512));
		HistoryWrapper = new(Rectangle.At(1408, 824, 512, 256));

		NestingContext.Add(menu);
		NestingContext.Add(arenaWrapper);
		NestingContext.Add(SpawnsWrapper);
		NestingContext.Add(HistoryWrapper);
	}

	public SpawnsWrapper SpawnsWrapper { get; }
	public HistoryWrapper HistoryWrapper { get; }

	public void SetSpawnset()
	{
		SpawnsWrapper.SetSpawnset();
	}

	public void SetHistory()
	{
		HistoryWrapper.SetHistory();
	}
}
