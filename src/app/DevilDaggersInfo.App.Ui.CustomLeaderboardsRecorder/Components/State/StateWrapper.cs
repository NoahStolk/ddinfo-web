using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Utils;
using System.Security.Cryptography;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.State;

public class StateWrapper : AbstractComponent
{
#pragma warning disable S1450 // Cannot change this into a local. The events would not be raised.
	private FileSystemWatcher? _survivalFileWatcher;
#pragma warning restore S1450

	public void Initialize()
	{
		UpdateActiveSpawnsetBasedOnHash();

		_survivalFileWatcher = new(UserSettings.ModsDirectory, "survival");
		_survivalFileWatcher.NotifyFilter = NotifyFilters.CreationTime
			| NotifyFilters.DirectoryName
			| NotifyFilters.FileName
			| NotifyFilters.LastWrite
			| NotifyFilters.Size;
		_survivalFileWatcher.IncludeSubdirectories = true; // This needs to be enabled for some reason.
		_survivalFileWatcher.EnableRaisingEvents = true;
		_survivalFileWatcher.Changed += (_, _) => UpdateActiveSpawnsetBasedOnHash();
		_survivalFileWatcher.Deleted += (_, _) => UpdateActiveSpawnsetBasedOnHash();
		_survivalFileWatcher.Created += (_, _) => UpdateActiveSpawnsetBasedOnHash();
		_survivalFileWatcher.Renamed += (_, _) => UpdateActiveSpawnsetBasedOnHash();
		_survivalFileWatcher.Error += (_, _) => UpdateActiveSpawnsetBasedOnHash();

		void UpdateActiveSpawnsetBasedOnHash()
		{
			if (!File.Exists(UserSettings.ModsSurvivalPath))
			{
				StateManager.Dispatch(new SetActiveSpawnset(null));
			}
			else
			{
				try
				{
					byte[] fileContents = File.ReadAllBytes(UserSettings.ModsSurvivalPath);
					byte[] fileHash = MD5.HashData(fileContents);
					AsyncHandler.Run(s => StateManager.Dispatch(new SetActiveSpawnset(s?.Name)), () => FetchSpawnsetByHash.HandleAsync(fileHash));
				}
				catch (Exception ex)
				{
					Root.Dependencies.Log.Error(ex, "Failed to update active spawnset based on hash.");
				}
			}
		}
	}
}
