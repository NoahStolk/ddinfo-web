using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.SurvivalEditor;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetHistory;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetMenu;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Silk.NET.GLFW;
using Warp;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditorMainLayout : Layout, ISurvivalEditorMainLayout
{
	public SurvivalEditorMainLayout()
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
		SpawnsWrapper.InitializeContent();
	}

	public void SetHistory()
	{
		HistoryWrapper.InitializeContent();
	}

	public void Update()
	{
		if (!Input.IsKeyHeld(Keys.ControlLeft) && !Input.IsKeyHeld(Keys.ControlRight))
			return;

		if (Input.IsKeyPressed(Keys.Z))
			SpawnsetHistoryManager.Undo();
		else if (Input.IsKeyPressed(Keys.Y))
			SpawnsetHistoryManager.Redo();
	}

	public void Render()
	{
		Root.Game.UiRenderer.RenderTopLeft(new(WindowWidth, WindowHeight), default, -100, new(0.1f));
	}
}