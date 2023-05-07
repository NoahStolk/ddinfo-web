using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.App.Engine.Maths.Numerics;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboards.LeaderboardList;

public static class LeaderboardListViewChild
{
	public static void Render()
	{
		if (LeaderboardListChild.IsLoading)
		{
			ImGui.TextColored(Color.Yellow, "Loading...");
		}
		else
		{
			ImGui.BeginChild("LeaderboardList");

			ImGui.Text($"Page {LeaderboardListChild.PageIndex + 1} of {LeaderboardListChild.TotalPages}");

			RenderTable();

			ImGui.EndChild();
		}
	}

	private static void RenderTable()
	{
		ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new Vector2(4, 1));
		if (ImGui.BeginTable("LeaderboardListTable", 8, ImGuiTableFlags.None))
		{
			// TODO: Clickable.
			ImGui.TableSetupColumn("Name");
			ImGui.TableSetupColumn("Author");
			ImGui.TableSetupColumn("Criteria");
			ImGui.TableSetupColumn("Score");
			ImGui.TableSetupColumn("Next dagger");
			ImGui.TableSetupColumn("Rank");
			ImGui.TableSetupColumn("Players");
			ImGui.TableSetupColumn("World record");
			ImGui.TableHeadersRow();

			foreach (GetCustomLeaderboardForOverview lb in LeaderboardListChild.PagedCustomLeaderboards)
			{
				ImGui.TableNextRow();
				ImGui.TableNextColumn();

				bool temp = true;
				if (ImGui.Selectable(lb.SpawnsetName, ref temp, ImGuiSelectableFlags.SpanAllColumns))
				{

				}

				ImGui.TableNextColumn();

				ImGui.Text(lb.SpawnsetAuthorName);
				ImGui.TableNextColumn();

				ImGui.Text(lb.Criteria.Count.ToString());
				ImGui.TableNextColumn();

				ImGui.Text(lb.SelectedPlayerStats?.Time.ToString() ?? "-");
				ImGui.TableNextColumn();

				ImGui.Text(lb.SelectedPlayerStats?.NextDagger?.Time.ToString() ?? "-");
				ImGui.TableNextColumn();

				ImGui.Text(lb.SelectedPlayerStats?.Rank.ToString() ?? "-");
				ImGui.TableNextColumn();

				ImGui.Text(lb.PlayerCount.ToString());
				ImGui.TableNextColumn();

				ImGui.Text(lb.WorldRecord?.Time.ToString() ?? "-");
				ImGui.TableNextColumn();
			}

			ImGui.PopStyleVar();
			ImGui.EndTable();
		}
	}
}
