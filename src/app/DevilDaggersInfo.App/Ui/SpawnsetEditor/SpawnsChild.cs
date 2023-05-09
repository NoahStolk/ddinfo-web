using DevilDaggersInfo.App.Extensions;
using DevilDaggersInfo.App.Ui.SpawnsetEditor.State;
using DevilDaggersInfo.App.Ui.SpawnsetEditor.Utils;
using DevilDaggersInfo.Common;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Core.Spawnset.Extensions;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Structs;
using ImGuiNET;
using Silk.NET.Input;
using System.Collections.Immutable;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SpawnsetEditor;

public static class SpawnsChild
{
	private static readonly bool[] _selected = new bool[2000]; // TODO: Make this dynamic.
	private static readonly string[] _enemyNames = Enum.GetValues<EnemyType>().Select(et => et.ToString()).ToArray();

	private static int _lastSelectedIndex = -1;
	private static float _editDelay;
	private static bool _delayEdited;

	private static int _addEnemyTypeIndex;
	private static float _addDelay;

	public static void Render()
	{
		ImGui.BeginChild("SpawnsChild", new(400 - 8, 768 - 64));

		ImGui.BeginChild("SpawnsListChild", new(400 - 8, 768 - 144));
		RenderSpawnsTable();
		ImGui.EndChild();

		ImGui.BeginChild("SpawnControlsChild", new(400 - 8, 72));

		ImGui.BeginChild("AddAndInsertButtons", new(72, 72));
		if (ImGui.Button("Add", new(64, 32)))
		{
			EnemyType enemyType = _addEnemyTypeIndex is >= 0 and <= 9 ? (EnemyType)_addEnemyTypeIndex : EnemyType.Empty;
			SpawnsetState.Spawnset = SpawnsetState.Spawnset with { Spawns = SpawnsetState.Spawnset.Spawns.Add(new(enemyType, _addDelay)) };
			SpawnsetHistoryUtils.Save(SpawnsetEditType.SpawnAdd);
		}

		if (ImGui.Button("Insert", new(64, 32)))
		{
			int selectedIndex = Array.IndexOf(_selected, true);
			if (selectedIndex == -1)
				selectedIndex = 0;

			EnemyType enemyType = _addEnemyTypeIndex is >= 0 and <= 9 ? (EnemyType)_addEnemyTypeIndex : EnemyType.Empty;
			SpawnsetState.Spawnset = SpawnsetState.Spawnset with { Spawns = SpawnsetState.Spawnset.Spawns.Insert(selectedIndex, new(enemyType, _addDelay)) };
			SpawnsetHistoryUtils.Save(SpawnsetEditType.SpawnInsert);
		}

		ImGui.EndChild();

		ImGui.SameLine();

		ImGui.BeginChild("AddSpawnControls");
		ImGui.Combo("Enemy", ref _addEnemyTypeIndex, _enemyNames, _enemyNames.Length);
		ImGui.InputFloat("Delay", ref _addDelay, 1, 2, "%.4f");
		ImGui.EndChild();

		ImGui.EndChild();

		ImGui.EndChild();
	}

	private static void RenderSpawnsTable()
	{
		ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new Vector2(4, 1));
		if (ImGui.BeginTable("SpawnsTable", 6, ImGuiTableFlags.None))
		{
			ImGuiIOPtr io = ImGui.GetIO();

			bool isFocused = true; // TODO: Get this from ImGui somehow.
			if (isFocused)
			{
				if (io.KeyCtrl)
				{
					if (io.KeysDown[(int)Key.A])
						Array.Fill(_selected, true);
					else if (io.KeysDown[(int)Key.D])
						Array.Fill(_selected, false);
				}

				if (io.KeysDown[(int)Key.Delete])
				{
					SpawnsetState.Spawnset = SpawnsetState.Spawnset with { Spawns = SpawnsetState.Spawnset.Spawns.Where((_, i) => !_selected[i]).ToImmutableArray() };
					Array.Fill(_selected, false);
					SpawnsetHistoryUtils.Save(SpawnsetEditType.SpawnDelete);
				}
			}

			ImGui.TableSetupColumn("#", ImGuiTableColumnFlags.WidthFixed, 16);
			ImGui.TableSetupColumn("Enemy", ImGuiTableColumnFlags.WidthFixed, 72);
			ImGui.TableSetupColumn("Time", ImGuiTableColumnFlags.WidthFixed, 72);
			ImGui.TableSetupColumn("Delay", ImGuiTableColumnFlags.WidthFixed, 72);
			ImGui.TableSetupColumn("Gems", ImGuiTableColumnFlags.WidthFixed, 48);
			// ImGui.SameLine();
			// ImGui.Text("(?)");
			// if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
			// 	ImGui.SetTooltip("The amount of gems an enemy drops when killed without farming.\nThis is also the amount of gems that will be added to the total gems counter.");

			ImGui.TableSetupColumn("Total", ImGuiTableColumnFlags.WidthFixed, 96);
			ImGui.TableHeadersRow();

			foreach (SpawnUiEntry spawn in EditSpawnContext.GetFrom(SpawnsetState.Spawnset))
			{
				ImGui.TableNextRow();
				ImGui.TableNextColumn();

				if (ImGui.Selectable(spawn.Index.ToString(), ref _selected[spawn.Index], ImGuiSelectableFlags.SpanAllColumns))
				{
					if (!io.KeyCtrl)
					{
						Array.Clear(_selected);
						_selected[spawn.Index] = true;
					}

					if (io.KeyShift && _lastSelectedIndex != -1)
					{
						int start = Math.Clamp(Math.Min(spawn.Index, _lastSelectedIndex), 0, _selected.Length - 1);
						int end = Math.Clamp(Math.Max(spawn.Index, _lastSelectedIndex), 0, _selected.Length - 1);
						for (int i = start; i <= end; i++)
							_selected[i] = true;
					}

					_lastSelectedIndex = spawn.Index;
				}

				EditContextItem(spawn);

				ImGui.TableNextColumn();

				ImGui.TextColored(spawn.EnemyType.GetColor(GameConstants.CurrentVersion), spawn.EnemyType.ToString());
				ImGui.TableNextColumn();

				ImGui.Text(spawn.Seconds.ToString(StringFormats.TimeFormat));
				ImGui.TableNextColumn();

				ImGui.Text(spawn.Delay.ToString(StringFormats.TimeFormat));
				ImGui.TableNextColumn();

				ImGui.Text(spawn.NoFarmGems == 0 ? "-" : $"+{spawn.NoFarmGems}");
				ImGui.TableNextColumn();

				ImGui.TextColored(spawn.GemState.HandLevel.GetColor(), spawn.GemState.Value.ToString());
				ImGui.TableNextColumn();
			}

			ImGui.PopStyleVar();
			ImGui.EndTable();
		}
	}

	private static void EditContextItem(SpawnUiEntry spawn)
	{
		bool saved = false;
		if (ImGui.BeginPopupContextItem(spawn.Index.ToString()))
		{
			if (!_delayEdited)
				_editDelay = (float)spawn.Delay;

			ImGui.Text($"Edit #{spawn.Index} ({spawn.EnemyType} at {spawn.Seconds.ToString(StringFormats.TimeFormat)})");

			foreach (EnemyType enemyType in Enum.GetValues<EnemyType>())
			{
				Color color = enemyType.GetColor(GameConstants.CurrentVersion);
				ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(0, 0, 0, 1));
				ImGui.PushStyleColor(ImGuiCol.Button, color);
				ImGui.PushStyleColor(ImGuiCol.ButtonHovered, color + new Vector4(0.3f, 0.3f, 0.3f, 0));
				ImGui.PushStyleColor(ImGuiCol.ButtonActive, color + new Vector4(0.5f, 0.5f, 0.5f, 0));

				if (ImGui.Button(enemyType.ToString(), new(96, 18)))
				{
					SaveEditedSpawn(spawn.Index, enemyType, _editDelay);
					saved = true;
				}

				ImGui.PopStyleColor();
				ImGui.PopStyleColor();
				ImGui.PopStyleColor();
				ImGui.PopStyleColor();
			}

			ImGui.InputFloat("Delay", ref _editDelay, 1, 5, "%.4f");
			if (!saved && Math.Abs(_editDelay - spawn.Delay) > 0.0001f)
				_delayEdited = true;

			if (ImGui.Button("Save", new(128, 20)))
				SaveEditedSpawn(spawn.Index, spawn.EnemyType, _editDelay);

			ImGui.EndPopup();
		}

		static void SaveEditedSpawn(int spawnIndex, EnemyType enemyType, float delay)
		{
			SpawnsetState.Spawnset = SpawnsetState.Spawnset with
			{
				Spawns = SpawnsetState.Spawnset.Spawns.SetItem(spawnIndex, new(enemyType, delay)),
			};

			SpawnsetHistoryUtils.Save(SpawnsetEditType.SpawnEdit);
			ImGui.CloseCurrentPopup();
			_delayEdited = false;
		}
	}
}