using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.App.Ui.SurvivalEditor.EditorSpawns;
using DevilDaggersInfo.Common;
using DevilDaggersInfo.Core.Spawnset.Extensions;
using DevilDaggersInfo.Core.Wiki;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class SpawnsWindow
{
	public static void Render()
	{
		ImGui.SetNextWindowSize(new(384, 512));
		ImGui.Begin("Spawns", ImGuiWindowFlags.ChildWindow | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse);

		if (ImGui.BeginTable("SpawnsTable", 5, ImGuiTableFlags.None))
		{
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

				ImGui.TableSetColumnIndex(0);
				ImGui.TextColored(spawn.EnemyType.GetColor(GameConstants.CurrentVersion), spawn.EnemyType.ToString());

				ImGui.TableSetColumnIndex(1);
				ImGui.Text(spawn.Seconds.ToString(StringFormats.TimeFormat));

				ImGui.TableSetColumnIndex(2);
				ImGui.Text(spawn.Delay.ToString(StringFormats.TimeFormat));

				ImGui.TableSetColumnIndex(3);
				ImGui.Text(spawn.NoFarmGems == 0 ? "-" : $"+{spawn.NoFarmGems}");

				ImGui.TableSetColumnIndex(4);
				ImGui.TextColored(spawn.GemState.HandLevel.GetColor(), spawn.GemState.Value.ToString());
			}

			ImGui.EndTable();
		}

		ImGui.End();
	}
}
