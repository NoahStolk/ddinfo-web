using DevilDaggersInfo.App.Ui.ReplayEditor.State;
using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.ReplayEditor;

public static class ReplayEditorMenu
{
	public static void Render()
	{
		if (ImGui.BeginMenuBar())
		{
			if (ImGui.BeginMenu("File"))
			{
				RenderFileMenu();
				ImGui.EndMenu();
			}

			ImGui.EndMenuBar();
		}

		ImGuiIOPtr io = ImGui.GetIO();
		if (io.KeyCtrl)
		{
			// TODO: Fix manual mapping?
			if (ImGui.IsKeyPressed(ImGuiKey.N) || ImGui.IsKeyPressed((ImGuiKey)78))
				NewReplay();
			else if (ImGui.IsKeyPressed(ImGuiKey.O) || ImGui.IsKeyPressed((ImGuiKey)79))
				OpenReplay();
			else if (ImGui.IsKeyPressed(ImGuiKey.S) || ImGui.IsKeyPressed((ImGuiKey)83))
				SaveReplay();
		}

		if (ImGui.IsKeyPressed(ImGuiKey.Escape) || ImGui.IsKeyPressed((ImGuiKey)526))
			Close();
	}

	private static void RenderFileMenu()
	{
		if (ImGui.MenuItem("New", "Ctrl+N"))
			NewReplay();

		if (ImGui.MenuItem("Open", "Ctrl+O"))
			OpenReplay();

		if (ImGui.MenuItem("Save", "Ctrl+S"))
			SaveReplay();

		ImGui.Separator();

		if (ImGui.MenuItem("Close", "Esc"))
			Close();
	}

	private static void NewReplay()
	{
		ReplayState.ReplayName = "(untitled)";
		ReplayState.Replay = ReplayBinary<LocalReplayBinaryHeader>.CreateDefault();
	}

	private static void OpenReplay()
	{
		string? filePath = Root.NativeFileSystemService.CreateOpenFileDialog("Open replay file", "Devil Daggers replay files (*.ddreplay)|*.ddreplay");
		if (filePath == null)
			return;

		byte[] fileContents;
		try
		{
			fileContents = File.ReadAllBytes(filePath);
		}
		catch (Exception ex)
		{
			// TODO: Log exception.
			Modals.ShowError = true;
			Modals.ErrorText = $"Could not open file '{filePath}'.";
			return;
		}

		if (ReplayBinary<LocalReplayBinaryHeader>.TryParse(fileContents, out ReplayBinary<LocalReplayBinaryHeader>? replayBinary))
		{
			ReplayState.ReplayName = Path.GetFileName(filePath);
			ReplayState.Replay = replayBinary;
		}
		else
		{
			Modals.ShowError = true;
			Modals.ErrorText = $"The file '{filePath}' could not be parsed as a local replay.";
			return;
		}

		ReplayEditorWindow.Time = 0;

		ReplaySimulation replaySimulation = ReplaySimulationBuilder.Build(ReplayState.Replay);
		Scene.ReplayArenaScene.Spawnset = ReplayState.Replay.Header.Spawnset;
		Scene.ReplayArenaScene.SetPlayerMovement(replaySimulation);
	}

	private static void SaveReplay()
	{
		string? filePath = Root.NativeFileSystemService.CreateSaveFileDialog("Save replay file", "Devil Daggers replay files (*.ddreplay)|*.ddreplay");
		if (filePath != null)
			File.WriteAllBytes(filePath, ReplayState.Replay.Compile());
	}

	private static void Close()
	{
		UiRenderer.Layout = LayoutType.Main;
	}
}
