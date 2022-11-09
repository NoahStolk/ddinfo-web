using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.SurvivalEditor;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using DevilDaggersInfo.App.Ui.Base.Settings;
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

	private readonly KeySubmitter _keySubmitter = new();

	public SurvivalEditorMainLayout()
		: base(Constants.Full)
	{
		Menu menu = new(new Rectangle(0, 0, 1024, 768));
		_arenaWrapper = new(Rectangle.At(400, 24, 400, 400));
		_spawnsWrapper = new(Rectangle.At(0, 24, 384, 640));
		SpawnEditor spawnEditor = new(Rectangle.At(0, 664, 384, 128));
		_historyWrapper = new(Rectangle.At(768, 512, 256, 256));
		_settingsWrapper = new(Rectangle.At(804, 24, 216, 256));

		NestingContext.Add(menu);
		NestingContext.Add(_arenaWrapper);
		NestingContext.Add(_spawnsWrapper);
		NestingContext.Add(spawnEditor);
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
		RenderBatchCollector.RenderRectangleTopLeft(new(WindowWidth, WindowHeight), default, -100, Color.Gray(0.1f));
	}
}
