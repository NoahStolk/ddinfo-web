using DevilDaggersInfo.App.Scenes;
using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboards;

public static class CustomLeaderboards3DWindow
{
	private static readonly FramebufferData _framebufferData = new();

	private static SpawnsetBinary _spawnset = SpawnsetBinary.CreateDefault();

	private static ArenaScene? _arenaScene;

	private static ArenaScene ArenaScene => _arenaScene ?? throw new InvalidOperationException("Scenes are not initialized.");

	public static void InitializeScene()
	{
		_arenaScene = new(static () => _spawnset, false, false);
	}

	public static void LoadReplay(ReplayBinary<LocalReplayBinaryHeader> replayBinary)
	{
		_spawnset = replayBinary.Header.Spawnset;

		ReplaySimulation replaySimulation = ReplaySimulationBuilder.Build(replayBinary);
		ArenaScene.SetPlayerMovement(replaySimulation);
	}

	public static void Update(float delta)
	{
		throw new NotImplementedException();
	}

	public static void Render(float delta)
	{
		ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, Constants.MinWindowSize / 2);
		if (ImGui.Begin("3D Replay Viewer"))
		{
			Vector2 framebufferSize = ImGui.GetWindowSize() - new Vector2(32, 48);
			_framebufferData.ResizeIfNecessary((int)framebufferSize.X, (int)framebufferSize.Y);

			Vector2 cursorScreenPos = ImGui.GetCursorScreenPos();
			ArenaScene.Camera.FramebufferOffset = cursorScreenPos;

			_framebufferData.RenderArena(delta, ArenaScene);

			ImDrawListPtr drawList = ImGui.GetWindowDrawList();
			drawList.AddImage((IntPtr)_framebufferData.TextureHandle, cursorScreenPos, cursorScreenPos + new Vector2(_framebufferData.Width, _framebufferData.Height), Vector2.UnitY, Vector2.UnitX);

			ImGui.End();
		}

		ImGui.PopStyleVar();
	}
}
