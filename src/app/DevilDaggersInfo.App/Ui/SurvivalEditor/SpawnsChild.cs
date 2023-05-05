using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.App.Ui.SurvivalEditor.State;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.Common;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Core.Spawnset.Extensions;
using DevilDaggersInfo.Core.Wiki;
using ImGuiNET;
using Silk.NET.Input;
using System.Collections.Immutable;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class SpawnsChild
{
	private static readonly bool[] _selected = new bool[2000]; // TODO: Make this dynamic.
	private static int _lastSelectedIndex = -1;

	public static void Render()
	{
		ImGui.BeginChild("SpawnsChild", new(400 - 8, 768 - 64));

		ImGui.BeginChild("SpawnsListChild", new(400 - 8, 768 - 136));
		RenderSpawnsTable();
		ImGui.EndChild();

		ImGui.BeginChild("SpawnControlsChild", new(400 - 8, 64));
		ImGui.Button("Add", new(64, 32));
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
					SpawnsetState.Spawnset = SpawnsetState.Spawnset with
					{
						Spawns = SpawnsetState.Spawnset.Spawns.Where((_, i) => !_selected[i]).ToImmutableArray(),
					};
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

				ImGui.TableNextColumn();

				if (ImGui.BeginPopupContextItem(spawn.Index.ToString()))
				{
					ImGui.Text($"Edit #{spawn.Index} ({spawn.EnemyType} at {spawn.Seconds.ToString(StringFormats.TimeFormat)})");

 #pragma warning disable S3267
					foreach (EnemyType enemyType in Enum.GetValues<EnemyType>())
 #pragma warning restore S3267
					{
						if (ImGui.Button(enemyType.ToString(), new(96, 16)))
						{
							Spawn oldSpawn = SpawnsetState.Spawnset.Spawns[spawn.Index];
							SpawnsetState.Spawnset = SpawnsetState.Spawnset with
							{
								Spawns = SpawnsetState.Spawnset.Spawns.SetItem(spawn.Index, new(enemyType, oldSpawn.Delay)),
							};

							SpawnsetHistoryUtils.Save(SpawnsetEditType.SpawnEdit);
							ImGui.CloseCurrentPopup();
						}
					}

					ImGui.EndPopup();
				}

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
}
