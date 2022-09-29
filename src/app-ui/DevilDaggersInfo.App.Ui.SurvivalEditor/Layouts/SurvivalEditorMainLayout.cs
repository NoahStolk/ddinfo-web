using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.SurvivalEditor;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetHistory;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSettings;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Silk.NET.GLFW;
using Warp;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditorMainLayout : Layout, ISurvivalEditorMainLayout
{
	private readonly SpawnsWrapper _spawnsWrapper;
	private readonly HistoryWrapper _historyWrapper;
	private readonly SettingsWrapper _settingsWrapper;

	public SurvivalEditorMainLayout()
		: base(Constants.Full)
	{
		Menu menu = new(new(0, 0, 1024, 16));
		ArenaWrapper arenaWrapper = new(Rectangle.At(400, 64, 400, 512));
		_spawnsWrapper = new(Rectangle.At(0, 64, 384, 512));
		_historyWrapper = new(Rectangle.At(768, 512, 256, 256));
		_settingsWrapper = new(Rectangle.At(800, 64, 224, 256));

		NestingContext.Add(menu);
		NestingContext.Add(arenaWrapper);
		NestingContext.Add(_spawnsWrapper);
		NestingContext.Add(_historyWrapper);
		NestingContext.Add(_settingsWrapper);
	}

	public void SetSpawnset()
	{
		_spawnsWrapper.InitializeContent();
		_settingsWrapper.SetSpawnset();
	}

	public void SetHistory()
	{
		_historyWrapper.InitializeContent();
	}

	public void Update()
	{
		if (!Input.IsKeyHeld(Keys.ControlLeft) && !Input.IsKeyHeld(Keys.ControlRight))
			return;

		if (Input.IsKeyPressed(Keys.Z))
			SpawnsetHistoryManager.Undo();
		else if (Input.IsKeyPressed(Keys.Y))
			SpawnsetHistoryManager.Redo();
		else if (Input.IsKeyPressed(Keys.N))
			StateManager.NewSpawnset();
		else if (Input.IsKeyPressed(Keys.O))
			LayoutManager.ToSurvivalEditorOpenLayout();
		else if (Input.IsKeyPressed(Keys.S))
			LayoutManager.ToSurvivalEditorSaveLayout();
	}

	public void Render3d()
	{
	}

	public void Render()
	{
		Root.Game.UiRenderer.RenderTopLeft(new(WindowWidth, WindowHeight), default, -100, new(0.1f));
	}

	public void RenderText()
	{
	}
}
