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
		ImGui.Begin("Spawns");

		if (ImGui.BeginTable("table1", 5, ImGuiTableFlags.None))
		{
			ImGui.TableSetupColumn("Enemy");
			ImGui.TableSetupColumn("Time");
			ImGui.TableSetupColumn("Delay");
			ImGui.TableSetupColumn("Gems");
			// ImGui.SameLine();
			// ImGui.Text("(?)");
			// if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
			// 	ImGui.SetTooltip("The amount of gems an enemy drops when killed without farming.\nThis is also the amount of gems that will be added to the total gems counter.");

			ImGui.TableSetupColumn("Total gems");
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
