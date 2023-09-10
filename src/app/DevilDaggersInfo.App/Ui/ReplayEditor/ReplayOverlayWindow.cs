using DevilDaggersInfo.App.Ui.ReplayEditor.Utils;
using DevilDaggersInfo.App.ZeroAllocation;
using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.ReplayEditor;

public static class ReplayOverlayWindow
{
	private static ReplaySimulation? _replaySimulation;

	public static void Render()
	{
		if (!GameMemoryServiceWrapper.Scan() || !Root.GameMemoryService.IsInitialized)
			return;

		if (Root.GameMemoryService.MainBlock.Status != 8)
			return;

		if (_replaySimulation == null)
		{
			byte[] replayBytes = Root.GameMemoryService.ReadReplayFromMemory();
			if (!ReplayBinary<LocalReplayBinaryHeader>.TryParse(replayBytes, out ReplayBinary<LocalReplayBinaryHeader>? replayBinary))
				return;

			_replaySimulation = ReplaySimulationBuilder.Build(replayBinary);
		}

		ImGui.SetNextWindowPos(new(32, 32));
		ImGui.SetNextWindowSize(new(256, 192));
		if (ImGui.Begin("Replay Inputs", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoNavFocus))
		{
			ImGui.Text("Replay inputs");

			float time = Root.GameMemoryService.MainBlock.Time;
			int tick = TimeUtils.TimeToTick(time, 0);

			ImGui.Text(UnsafeSpan.Get($"{time + Root.GameMemoryService.MainBlock.StartTimer:0.0000}"));

			PlayerInputSnapshot snapshot = default;
			if (tick >= 0 && tick < _replaySimulation.InputSnapshots.Count)
				snapshot = _replaySimulation.InputSnapshots[tick];

			Vector2 origin = ImGui.GetCursorScreenPos();
			ReplayInputs.Render(origin, snapshot);
		}

		ImGui.End(); // End Replay Inputs
	}
}
