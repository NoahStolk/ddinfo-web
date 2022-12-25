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
using Warp.NET;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditorMainLayout : Layout, ISurvivalEditorMainLayout
{
	private readonly SpawnsWrapper _spawnsWrapper;
	private readonly ArenaWrapper _arenaWrapper;
	private readonly SettingsWrapper _settingsWrapper;

	private readonly History _history;

	private readonly KeySubmitter _keySubmitter = new();

	public SurvivalEditorMainLayout()
	{
		Menu menu = new(new PixelBounds(0, 0, 1024, 768));
		_spawnsWrapper = new(new PixelBounds(0, 24, 400, 640));
		_arenaWrapper = new(new PixelBounds(400, 24, 400, 400));
		SpawnEditor spawnEditor = new(new PixelBounds(0, 664, 384, 128));
		_history = new(new PixelBounds(768, 512, 256, 256));
		_settingsWrapper = new(new PixelBounds(804, 24, 216, 256));

		NestingContext.Add(menu);
		NestingContext.Add(_spawnsWrapper);
		NestingContext.Add(_arenaWrapper);
		NestingContext.Add(spawnEditor);
		NestingContext.Add(_history);
		NestingContext.Add(_settingsWrapper);
	}

	public void SetSpawnset(bool hasArenaChanges, bool hasSpawnsChanges, bool hasSettingsChanges)
	{
		if (hasArenaChanges)
			_arenaWrapper.SetSpawnset();

		if (hasSpawnsChanges || hasSettingsChanges)
			_spawnsWrapper.SetSpawnset();

		if (hasSettingsChanges)
			_settingsWrapper.SetSpawnset();
	}

	public void SetHistory()
	{
		_history.SetContent();
	}

	public void Update()
	{
		if (!Input.IsCtrlHeld())
			return;

		Keys? key = _keySubmitter.GetKey();
		if (key.HasValue)
		{
			if (key == Keys.Z)
				SpawnsetHistoryManager.Undo();
			else if (key == Keys.Y)
				SpawnsetHistoryManager.Redo();
		}

		if (Input.IsKeyPressed(Keys.N))
			StateManager.NewSpawnset();
		else if (Input.IsKeyPressed(Keys.O))
			LayoutManager.ToSurvivalEditorOpenLayout();
		else if (Input.IsKeyPressed(Keys.S))
			LayoutManager.ToSurvivalEditorSaveLayout();
		else if (Input.IsKeyPressed(Keys.R))
			StateManager.ReplaceSpawnset();
	}

	public void Render3d()
	{
	}

	public void Render()
	{
		Vector2i<int> windowSize = new(CurrentWindowState.Width, CurrentWindowState.Height);
		Root.Game.RectangleRenderer.Schedule(windowSize, windowSize / 2, -100, Color.Gray(0.1f));
	}
}
