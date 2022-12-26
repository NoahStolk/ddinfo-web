using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.App.Ui.Base.StateManagement.Base.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetHistory;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSettings;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;
using DevilDaggersInfo.Core.Spawnset;
using Silk.NET.GLFW;
using Warp.NET;
using Warp.NET.Ui;
using StateManager = DevilDaggersInfo.App.Ui.Base.StateManagement.StateManager;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditorMainLayout : Layout, IExtendedLayout
{
	private readonly KeySubmitter _keySubmitter = new();

	public SurvivalEditorMainLayout()
	{
		Menu menu = new(new PixelBounds(0, 0, 1024, 768));
		SpawnsWrapper spawnsWrapper = new(new PixelBounds(0, 24, 400, 640));
		ArenaWrapper arenaWrapper = new(new PixelBounds(400, 24, 400, 400));
		SpawnEditor spawnEditor = new(new PixelBounds(0, 664, 384, 128));
		HistoryScrollArea historyScrollArea = new(new PixelBounds(768, 512, 256, 256));
		SettingsWrapper settingsWrapper = new(new PixelBounds(804, 24, 216, 256));

		NestingContext.Add(menu);
		NestingContext.Add(spawnsWrapper);
		NestingContext.Add(arenaWrapper);
		NestingContext.Add(spawnEditor);
		NestingContext.Add(historyScrollArea);
		NestingContext.Add(settingsWrapper);

		StateManager.Subscribe<ReplaceCurrentlyActiveSpawnset>(_ => ReplaceSpawnset());
	}

	private void ReplaceSpawnset()
	{
		File.WriteAllBytes(UserSettings.ModsSurvivalPath, StateManager.SpawnsetState.Spawnset.ToBytes());
		Popup popup = new(this, "Successfully replaced current survival file");
		NestingContext.Add(popup);
	}

	public void Update()
	{
		if (!Input.IsCtrlHeld())
			return;

		Keys? key = _keySubmitter.GetKey();
		if (key.HasValue)
		{
			if (key == Keys.Z)
				StateManager.Dispatch(new SetSpawnsetHistoryIndex(StateManager.SpawnsetHistoryState.CurrentIndex - 1, StateManager.SpawnsetHistoryState.Count));
			else if (key == Keys.Y)
				StateManager.Dispatch(new SetSpawnsetHistoryIndex(StateManager.SpawnsetHistoryState.CurrentIndex + 1, StateManager.SpawnsetHistoryState.Count));
		}

		if (Input.IsKeyPressed(Keys.N))
			StateManager.Dispatch(new LoadSpawnset("(untitled)", SpawnsetBinary.CreateDefault()));
		else if (Input.IsKeyPressed(Keys.O))
			StateManager.Dispatch(new SetLayout(Root.Game.SurvivalEditorOpenLayout));
		else if (Input.IsKeyPressed(Keys.S))
			StateManager.Dispatch(new SetLayout(Root.Game.SurvivalEditorSaveLayout));
		else if (Input.IsKeyPressed(Keys.R))
			StateManager.Dispatch(new ReplaceCurrentlyActiveSpawnset());
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
