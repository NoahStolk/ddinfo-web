using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.States;

public static class StateManager
{
	public static SpawnsetState SpawnsetState { get; private set; } = SpawnsetState.GetDefault();

	public static ArenaEditorState ArenaEditorState { get; private set; } = ArenaEditorState.GetDefault();

	public static void NewSpawnset()
	{
		SetSpawnset("(untitled)", SpawnsetBinary.CreateDefault());
	}

	public static void OpenDefaultV3Spawnset()
	{
		SetSpawnset("(untitled)", ContentManager.Content.DefaultSpawnset);
	}

	public static void SetSpawnset(string spawnsetName, SpawnsetBinary spawnsetBinary)
	{
		SpawnsetState = new(spawnsetName, spawnsetBinary);

		Root.Game.SurvivalEditorMainLayout.SetSpawnset();
		SpawnsetHistoryManager.Reset();
	}

	public static void SetSpawnset(SpawnsetBinary spawnsetBinary)
	{
		SpawnsetState = SpawnsetState with { Spawnset = spawnsetBinary };

		Root.Game.SurvivalEditorMainLayout.SetSpawnset();
	}

	public static void SetArenaTool(ArenaTool arenaTool)
	{
		ArenaEditorState = ArenaEditorState with { ArenaTool = arenaTool };
	}

	public static void SetArenaSelectedHeight(float selectedHeight)
	{
		ArenaEditorState = ArenaEditorState with { SelectedHeight = selectedHeight };
	}
}
