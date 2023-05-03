using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.App.Ui.SurvivalEditor.EditorSpawns;
using DevilDaggersInfo.Common;
using DevilDaggersInfo.Core.Spawnset.Extensions;
using DevilDaggersInfo.Core.Wiki;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class SpawnsWindow
{
	private static readonly bool[] _selected = new bool[2000]; // TODO: Make this dynamic.

	public static void Render()
	{
		ImGui.SetNextWindowSize(new(400, 768 - 64));
		ImGui.Begin("Spawns", ImGuiWindowFlags.ChildWindow | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse);

		ImGui.BeginChild("SpawnsChild", new(400 - 8, 768 - 136));
		ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new Vector2(4, 1));
		if (ImGui.BeginTable("SpawnsTable", 6, ImGuiTableFlags.None))
		{
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

				ImGui.Selectable(spawn.Index.ToString(), ref _selected[spawn.Index], ImGuiSelectableFlags.SpanAllColumns);
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

		ImGui.EndChild();

		ImGui.Button("Add", new(64, 32));

		ImGui.End();
	}
}
