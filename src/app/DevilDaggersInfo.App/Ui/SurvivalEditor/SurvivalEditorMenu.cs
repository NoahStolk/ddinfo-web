using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.User.Settings;
using DevilDaggersInfo.App.Ui.SurvivalEditor.State;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.App.Utils;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;
using Silk.NET.Input;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class SurvivalEditorMenu
{
	private static readonly ImGuiIoState _keyN = new(false, (int)Key.N);
	private static readonly ImGuiIoState _keyO = new(false, (int)Key.O);
	private static readonly ImGuiIoState _keyS = new(false, (int)Key.S);
	private static readonly ImGuiIoState _keyR = new(false, (int)Key.R);

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
		_keyN.Update(io);
		_keyO.Update(io);
		_keyS.Update(io);
		_keyR.Update(io);

		if (io.KeyCtrl)
		{
			if (_keyN.JustPressed)
				NewSpawnset();
			else if (_keyO.JustPressed)
				OpenSpawnset();
			else if (_keyS.JustPressed)
				SaveSpawnset();
			else if (_keyR.JustPressed)
				ReplaceSpawnset();
		}
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

		if (ImGui.MenuItem("Close"))
			UiRenderer.Layout = LayoutType.Main;
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
}
