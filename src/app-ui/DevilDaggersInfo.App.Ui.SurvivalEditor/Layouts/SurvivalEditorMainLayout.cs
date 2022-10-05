using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.SurvivalEditor;
using DevilDaggersInfo.App.Ui.Base.Rendering;
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
	private readonly ArenaWrapper _arenaWrapper;

	public SurvivalEditorMainLayout()
		: base(Constants.Full)
	{
		Menu menu = new(new(0, 0, 1024, 16));
		_arenaWrapper = new(Rectangle.At(400, 16, 400, 512));
		_spawnsWrapper = new(Rectangle.At(0, 16, 384, 512));
		_historyWrapper = new(Rectangle.At(768, 512, 256, 256));
		_settingsWrapper = new(Rectangle.At(800, 16, 224, 256));

		NestingContext.Add(menu);
		NestingContext.Add(_arenaWrapper);
		NestingContext.Add(_spawnsWrapper);
		NestingContext.Add(_historyWrapper);
		NestingContext.Add(_settingsWrapper);
	}

	public void SetSpawnset(bool hasArenaChanges, bool hasSpawnsChanges, bool hasSettingsChanges)
	{
		if (hasArenaChanges)
			_arenaWrapper.SetSpawnset();

		if (hasSpawnsChanges || hasSettingsChanges)
			_spawnsWrapper.InitializeContent();

		if (hasSettingsChanges)
			_settingsWrapper.SetSpawnset();
	}

	public void SetHistory()
	{
		_historyWrapper.InitializeContent();
		_historyWrapper.SetScroll(-SpawnsetHistoryManager.Index * History.HistoryEntryHeight);
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
		RenderBatchCollector.RenderRectangleTopLeft(new(WindowWidth, WindowHeight), default, -100, Color.Gray(0.1f));
	}
}
