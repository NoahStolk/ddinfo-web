﻿using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.Base.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;

public static class SpawnsetFileUtils
{
	public static void OpenSpawnset()
	{
		string? filePath = Root.Dependencies.NativeFileSystemService.CreateOpenFileDialog("Open spawnset file", null);
		if (filePath == null)
			return;

		byte[] bytes = File.ReadAllBytes(filePath);
		if (SpawnsetBinary.TryParse(bytes, out SpawnsetBinary? spawnsetBinary))
		{
			StateManager.Dispatch(new LoadSpawnset(Path.GetFileName(filePath), spawnsetBinary));
			StateManager.Dispatch(new SetLayout(Root.Dependencies.SurvivalEditorMainLayout));
		}
		else
		{
			Root.Dependencies.NativeDialogService.ReportError("The file could not be parsed as a spawnset.");
		}
	}

	public static void SaveSpawnset()
	{
		string? filePath = Root.Dependencies.NativeFileSystemService.CreateSaveFileDialog("Save spawnset file", null);
		if (filePath == null)
			return;

		if (Directory.Exists(filePath))
		{
			Root.Dependencies.NativeDialogService.ReportError("Specified file path is an existing directory. Please specify a file path.");
			return;
		}

		if (File.Exists(filePath))
		{
			bool? result = Root.Dependencies.NativeDialogService.PromptYesNo("File already exists", "The specified file path already exists. Do you want to overwrite it?");
			if (result is null or false)
				return;
		}

		File.WriteAllBytes(filePath, StateManager.SpawnsetState.Spawnset.ToBytes());
	}

	public static void ReplaceSpawnset()
	{
		File.WriteAllBytes(UserSettings.ModsSurvivalPath, StateManager.SpawnsetState.Spawnset.ToBytes());
		Root.Dependencies.NativeDialogService.ReportMessage("Successfully replaced current survival file", "The current survival file has been replaced with the current spawnset.");
	}
}
