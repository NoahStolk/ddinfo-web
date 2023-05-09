using DevilDaggersInfo.App.Ui.SpawnsetEditor.State;
using DevilDaggersInfo.App.Ui.SpawnsetEditor.Utils;
using DevilDaggersInfo.App.User.Settings;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.SpawnsetEditor;

public static class SpawnsetEditorMenu
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
			// TODO: Prevent Ctrl+R from opening the replace dialog when the replace dialog is already open.
			// ... Or just ignore key "presses" when the key is already held down.
			if (ImGui.IsKeyPressed(ImGuiKey.N) || ImGui.IsKeyPressed((ImGuiKey)78))
				NewSpawnset();
			else if (ImGui.IsKeyPressed(ImGuiKey.O) || ImGui.IsKeyPressed((ImGuiKey)79))
				OpenSpawnset();
			else if (ImGui.IsKeyPressed(ImGuiKey.S) || ImGui.IsKeyPressed((ImGuiKey)83))
				SaveSpawnset();
			else if (ImGui.IsKeyPressed(ImGuiKey.R) || ImGui.IsKeyPressed((ImGuiKey)82))
				ReplaceSpawnset();
		}

		if (ImGui.IsKeyPressed(ImGuiKey.Escape) || ImGui.IsKeyPressed((ImGuiKey)526))
			Close();
	}

	private static void RenderFileMenu()
	{
		if (ImGui.MenuItem("New", "Ctrl+N"))
			NewSpawnset();

		if (ImGui.MenuItem("Open", "Ctrl+O"))
			OpenSpawnset();

		if (ImGui.MenuItem("Open default (V3)"))
			OpenDefaultSpawnset();

		if (ImGui.MenuItem("Save", "Ctrl+S"))
			SaveSpawnset();

		if (ImGui.MenuItem("Replace", "Ctrl+R"))
			ReplaceSpawnset();

		ImGui.Separator();

		if (ImGui.MenuItem("Close", "Esc"))
			Close();
	}

	private static void NewSpawnset()
	{
		SpawnsetState.SpawnsetName = "(untitled)";
		SpawnsetState.Spawnset = SpawnsetBinary.CreateDefault();
		SpawnsetHistoryUtils.Save(SpawnsetEditType.Reset);
	}

	private static void OpenSpawnset()
	{
		string? filePath = Root.NativeFileSystemService.CreateOpenFileDialog("Open spawnset file", null);
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

		if (SpawnsetBinary.TryParse(fileContents, out SpawnsetBinary? spawnsetBinary))
		{
			SpawnsetState.SpawnsetName = Path.GetFileName(filePath);
			SpawnsetState.Spawnset = spawnsetBinary;
		}
		else
		{
			Modals.ShowError = true;
			Modals.ErrorText = $"The file '{filePath}' could not be parsed as a spawnset.";
			return;
		}

		SpawnsetHistoryUtils.Save(SpawnsetEditType.Reset);
	}

	private static void OpenDefaultSpawnset()
	{
		SpawnsetState.SpawnsetName = "V3";
		SpawnsetState.Spawnset = ContentManager.Content.DefaultSpawnset.DeepCopy();
		SpawnsetHistoryUtils.Save(SpawnsetEditType.Reset);
	}

	private static void SaveSpawnset()
	{
		string? filePath = Root.NativeFileSystemService.CreateSaveFileDialog("Save spawnset file", null);
		if (filePath != null)
			File.WriteAllBytes(filePath, SpawnsetState.Spawnset.ToBytes());
	}

	private static void ReplaceSpawnset()
	{
		File.WriteAllBytes(UserSettings.ModsSurvivalPath, SpawnsetState.Spawnset.ToBytes());
		Modals.ShowReplacedSurvivalFile = true;
	}

	private static void Close()
	{
		UiRenderer.Layout = LayoutType.Main;
	}
}