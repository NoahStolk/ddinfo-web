using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.Api.App.Spawnsets;
using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Extensions;
using DevilDaggersInfo.App.Networking;
using DevilDaggersInfo.App.Networking.TaskHandlers;
using DevilDaggersInfo.App.Ui.CustomLeaderboards.LeaderboardList;
using DevilDaggersInfo.App.User.Cache;
using DevilDaggersInfo.App.User.Settings;
using DevilDaggersInfo.Common;
using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Objects;
using ImGuiNET;
using System.Diagnostics;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboards.Leaderboard;

public static class LeaderboardChild
{
	private static List<GetCustomEntry> _sortedEntries = new();

	private static LeaderboardData? _data;
	public static LeaderboardData? Data
	{
		get => _data;
		set
		{
			_data = value;
			_sortedEntries = value?.Leaderboard.SortedEntries ?? new();
		}
	}

	public static void Render()
	{
		ImGui.BeginChild("LeaderboardChild");

		if (Data == null)
			ImGui.Text("None selected");
		else
			RenderLeaderboard(Data);

		ImGui.EndChild();
	}

	private static void RenderLeaderboard(LeaderboardData data)
	{
		ImGui.Text(data.Leaderboard.SpawnsetName);

		if (ImGui.Button("Play", new(80, 20)))
		{
			AsyncHandler.Run(InstallSpawnset, () => FetchSpawnsetById.HandleAsync(data.SpawnsetId));
			void InstallSpawnset(GetSpawnset? spawnset)
			{
				if (spawnset == null)
				{
					Modals.ShowError = true;
					Modals.ErrorText = "Could not fetch spawnset.";
					return;
				}

				File.WriteAllBytes(UserSettings.ModsSurvivalPath, spawnset.FileBytes);
			}
		}

		ImGui.BeginChild("LeaderboardTableChild");
		RenderTable();
		ImGui.EndChild();
	}

	private static unsafe void RenderTable()
	{
		const ImGuiTableFlags flags = ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Hideable | ImGuiTableFlags.Sortable | ImGuiTableFlags.SortMulti | ImGuiTableFlags.RowBg | ImGuiTableFlags.BordersV | ImGuiTableFlags.NoBordersInBody;

		ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new Vector2(4, 1));
		if (ImGui.BeginTable("LeaderboardTable", 16, flags))
		{
			ImGui.TableSetupColumn("Rank", ImGuiTableColumnFlags.DefaultSort, 0, (int)LeaderboardSorting.Rank);
			ImGui.TableSetupColumn("Player", ImGuiTableColumnFlags.None, 0, (int)LeaderboardSorting.PlayerName);
			ImGui.TableSetupColumn("Time", ImGuiTableColumnFlags.None, 0, (int)LeaderboardSorting.Time);
			ImGui.TableSetupColumn("Enemies alive", ImGuiTableColumnFlags.None, 0, (int)LeaderboardSorting.EnemiesAlive);
			ImGui.TableSetupColumn("Enemies killed", ImGuiTableColumnFlags.None, 0, (int)LeaderboardSorting.EnemiesKilled);
			ImGui.TableSetupColumn("Gems collected", ImGuiTableColumnFlags.None, 0, (int)LeaderboardSorting.GemsCollected);
			ImGui.TableSetupColumn("Gems despawned", ImGuiTableColumnFlags.None, 0, (int)LeaderboardSorting.GemsDespawned);
			ImGui.TableSetupColumn("Gems eaten", ImGuiTableColumnFlags.None, 0, (int)LeaderboardSorting.GemsEaten);
			ImGui.TableSetupColumn("Accuracy", ImGuiTableColumnFlags.None, 0, (int)LeaderboardSorting.Accuracy);
			ImGui.TableSetupColumn("Death type", ImGuiTableColumnFlags.None, 0, (int)LeaderboardSorting.DeathType);
			ImGui.TableSetupColumn("Homing stored", ImGuiTableColumnFlags.None, 0, (int)LeaderboardSorting.HomingStored);
			ImGui.TableSetupColumn("Homing eaten", ImGuiTableColumnFlags.None, 0, (int)LeaderboardSorting.HomingEaten);
			ImGui.TableSetupColumn("Level 2", ImGuiTableColumnFlags.None, 0, (int)LeaderboardSorting.LevelUpTime2);
			ImGui.TableSetupColumn("Level 3", ImGuiTableColumnFlags.None, 0, (int)LeaderboardSorting.LevelUpTime3);
			ImGui.TableSetupColumn("Level 4", ImGuiTableColumnFlags.None, 0, (int)LeaderboardSorting.LevelUpTime4);
			ImGui.TableSetupColumn("Submit date", ImGuiTableColumnFlags.None, 0, (int)LeaderboardSorting.SubmitDate);
			ImGui.TableHeadersRow();

			ImGuiTableSortSpecsPtr sortsSpecs = ImGui.TableGetSortSpecs();
			if (sortsSpecs.NativePtr != (void*)0 && sortsSpecs.SpecsDirty)
			{
				Sort(sortsSpecs);
				sortsSpecs.SpecsDirty = false;
			}

			foreach (GetCustomEntry ce in _sortedEntries)
			{
				ImGui.TableNextRow();
				RenderCustomEntry(ce);
			}

			ImGui.PopStyleVar();
			ImGui.EndTable();
		}
	}

	private static void RenderCustomEntry(GetCustomEntry ce)
	{
		Vector2 iconSize = new(8);

		ImGui.TableNextColumn();

		ImGui.Text(ce.Rank.ToString("00"));

		ImGui.SameLine();
		ImGui.PushID(ce.Id);
		if (ImGui.ImageButton((IntPtr)Root.InternalResources.IconEyeTexture.Handle, iconSize))
			WatchInGame();

		ImGui.PopID();

		if (ImGui.IsItemHovered())
			ImGui.SetTooltip("Watch in game");

		ImGui.SameLine();
		ImGui.PushID(ce.Id);
		if (ImGui.ImageButton((IntPtr)Root.InternalResources.IconEyeTexture.Handle, iconSize))
			WatchInReplayViewer();

		ImGui.PopID();

		if (ImGui.IsItemHovered())
			ImGui.SetTooltip("Watch in replay viewer");

		ImGui.TableNextColumn();

		ImGui.TextColored(ce.PlayerId == UserCache.Model.PlayerId ? Color.Green : Color.White, ce.PlayerName);
		ImGui.TableNextColumn();

		ImGui.TextColored(CustomLeaderboardDaggerUtils.GetColor(ce.CustomLeaderboardDagger), ce.TimeInSeconds.ToString(StringFormats.TimeFormat));
		ImGui.TableNextColumn();

		ImGui.Text(ce.EnemiesAlive.ToString());
		ImGui.TableNextColumn();

		ImGui.Text(ce.EnemiesKilled.ToString());
		ImGui.TableNextColumn();

		ImGui.Text(ce.GemsCollected.ToString());
		ImGui.TableNextColumn();

		ImGui.Text(ce.GemsDespawned?.ToString() ?? "-");
		ImGui.TableNextColumn();

		ImGui.Text(ce.GemsEaten?.ToString() ?? "-");
		ImGui.TableNextColumn();

		ImGui.TextUnformatted(GetAccuracy(ce).ToString(StringFormats.AccuracyFormat));
		ImGui.TableNextColumn();

		Death? death = Deaths.GetDeathByLeaderboardType(GameConstants.CurrentVersion, ce.DeathType);
		ImGui.TextColored(death?.Color.ToEngineColor() ?? Color.White, death?.Name ?? "Unknown");
		ImGui.TableNextColumn();

		ImGui.Text(ce.HomingStored.ToString());
		ImGui.TableNextColumn();

		ImGui.Text(ce.HomingEaten?.ToString() ?? "-");
		ImGui.TableNextColumn();

		ImGui.Text(ce.LevelUpTime2InSeconds == 0 ? "-" : ce.LevelUpTime2InSeconds.ToString(StringFormats.TimeFormat));
		ImGui.TableNextColumn();

		ImGui.Text(ce.LevelUpTime3InSeconds == 0 ? "-" : ce.LevelUpTime3InSeconds.ToString(StringFormats.TimeFormat));
		ImGui.TableNextColumn();

		ImGui.Text(ce.LevelUpTime4InSeconds == 0 ? "-" : ce.LevelUpTime4InSeconds.ToString(StringFormats.TimeFormat));
		ImGui.TableNextColumn();

		ImGui.Text(ce.SubmitDate.ToString(StringFormats.DateTimeFormat));
		ImGui.TableNextColumn();

		void WatchInGame()
		{
			AsyncHandler.Run(Inject, () => FetchCustomEntryReplayById.HandleAsync(ce.Id));

			void Inject(GetCustomEntryReplayBuffer? getCustomEntryReplayBuffer)
			{
				if (getCustomEntryReplayBuffer == null)
				{
					Modals.ShowError = true;
					Modals.ErrorText = "Could not fetch replay.";
					return;
				}

				Root.GameMemoryService.WriteReplayToMemory(getCustomEntryReplayBuffer.Data);
			}
		}

		void WatchInReplayViewer()
		{
			AsyncHandler.Run(BuildReplayScene, () => FetchCustomEntryReplayById.HandleAsync(ce.Id));

			void BuildReplayScene(GetCustomEntryReplayBuffer? getCustomEntryReplayBuffer)
			{
				if (getCustomEntryReplayBuffer == null)
				{
					Modals.ShowError = true;
					Modals.ErrorText = "Could not fetch replay.";
					return;
				}

				ReplayBinary<LocalReplayBinaryHeader> replayBinary;
				try
				{
					replayBinary = new(getCustomEntryReplayBuffer.Data);
				}
				catch (Exception ex)
				{
					Modals.ShowError = true;
					Modals.ErrorText = "Could not parse replay.";
					return;
				}

				// StateManager.Dispatch(new SetLayout(Root.Dependencies.CustomLeaderboardsRecorderReplayViewer3dLayout));
				// StateManager.Dispatch(new BuildReplayScene(new[] { replayBinary }));
			}
		}
	}

	private static void Sort(ImGuiTableSortSpecsPtr sortsSpecs)
	{
		LeaderboardSorting sorting = (LeaderboardSorting)sortsSpecs.Specs.ColumnUserID;
		bool sortAscending = sortsSpecs.Specs.SortDirection == ImGuiSortDirection.Ascending;
		_sortedEntries = (sorting switch
		{
			LeaderboardSorting.Rank => sortAscending ? _sortedEntries.OrderBy(ce => ce.Rank) : _sortedEntries.OrderByDescending(ce => ce.Rank),
			LeaderboardSorting.PlayerName => sortAscending ? _sortedEntries.OrderBy(ce => ce.PlayerName.ToLower()) : _sortedEntries.OrderByDescending(ce => ce.PlayerName.ToLower()),
			LeaderboardSorting.Time => sortAscending ? _sortedEntries.OrderBy(ce => ce.TimeInSeconds) : _sortedEntries.OrderByDescending(ce => ce.TimeInSeconds),
			LeaderboardSorting.EnemiesAlive => sortAscending ? _sortedEntries.OrderBy(ce => ce.EnemiesAlive) : _sortedEntries.OrderByDescending(ce => ce.EnemiesAlive),
			LeaderboardSorting.EnemiesKilled => sortAscending ? _sortedEntries.OrderBy(ce => ce.EnemiesKilled) : _sortedEntries.OrderByDescending(ce => ce.EnemiesKilled),
			LeaderboardSorting.GemsCollected => sortAscending ? _sortedEntries.OrderBy(ce => ce.GemsCollected) : _sortedEntries.OrderByDescending(ce => ce.GemsCollected),
			LeaderboardSorting.GemsDespawned => sortAscending ? _sortedEntries.OrderBy(ce => ce.GemsDespawned) : _sortedEntries.OrderByDescending(ce => ce.GemsDespawned),
			LeaderboardSorting.GemsEaten => sortAscending ? _sortedEntries.OrderBy(ce => ce.GemsEaten) : _sortedEntries.OrderByDescending(ce => ce.GemsEaten),
			LeaderboardSorting.Accuracy => sortAscending ? _sortedEntries.OrderBy(GetAccuracy) : _sortedEntries.OrderByDescending(GetAccuracy),
			LeaderboardSorting.DeathType => sortAscending ? _sortedEntries.OrderBy(DeathSort) : _sortedEntries.OrderByDescending(DeathSort),
			LeaderboardSorting.HomingStored => sortAscending ? _sortedEntries.OrderBy(ce => ce.HomingStored) : _sortedEntries.OrderByDescending(ce => ce.HomingStored),
			LeaderboardSorting.HomingEaten => sortAscending ? _sortedEntries.OrderBy(ce => ce.HomingEaten) : _sortedEntries.OrderByDescending(ce => ce.HomingEaten),
			LeaderboardSorting.LevelUpTime2 => sortAscending ? _sortedEntries.OrderBy(ce => ce.LevelUpTime2InSeconds) : _sortedEntries.OrderByDescending(ce => ce.LevelUpTime2InSeconds),
			LeaderboardSorting.LevelUpTime3 => sortAscending ? _sortedEntries.OrderBy(ce => ce.LevelUpTime3InSeconds) : _sortedEntries.OrderByDescending(ce => ce.LevelUpTime3InSeconds),
			LeaderboardSorting.LevelUpTime4 => sortAscending ? _sortedEntries.OrderBy(ce => ce.LevelUpTime4InSeconds) : _sortedEntries.OrderByDescending(ce => ce.LevelUpTime4InSeconds),
			LeaderboardSorting.SubmitDate => sortAscending ? _sortedEntries.OrderBy(ce => ce.SubmitDate) : _sortedEntries.OrderByDescending(ce => ce.SubmitDate),
			_ => throw new UnreachableException(),
		}).ToList();

		static string? DeathSort(GetCustomEntry ce)
		{
			return Deaths.GetDeathByLeaderboardType(GameConstants.CurrentVersion, ce.DeathType)?.Name;
		}
	}

	private static float GetAccuracy(GetCustomEntry ce)
	{
		return ce.DaggersFired == 0 ? 0 : ce.DaggersHit / (float)ce.DaggersFired;
	}

	public sealed record LeaderboardData(GetCustomLeaderboard Leaderboard, int SpawnsetId);
}
