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

	private static unsafe void RenderTable()
	{
		const ImGuiTableFlags flags = ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Hideable | ImGuiTableFlags.Sortable | ImGuiTableFlags.SortMulti | ImGuiTableFlags.RowBg | ImGuiTableFlags.BordersV | ImGuiTableFlags.NoBordersInBody;

		ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new Vector2(4, 1));
		if (ImGui.BeginTable("LeaderboardListTable", 8, flags))
		{
			ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.DefaultSort, 0, (int)LeaderboardListSorting.Name);
			ImGui.TableSetupColumn("Author", ImGuiTableColumnFlags.None, 0, (int)LeaderboardListSorting.Author);
			ImGui.TableSetupColumn("Criteria", ImGuiTableColumnFlags.None, 0, (int)LeaderboardListSorting.Criteria);
			ImGui.TableSetupColumn("Score", ImGuiTableColumnFlags.None, 0, (int)LeaderboardListSorting.Score);
			ImGui.TableSetupColumn("Next dagger", ImGuiTableColumnFlags.None, 0, (int)LeaderboardListSorting.NextDagger);
			ImGui.TableSetupColumn("Rank", ImGuiTableColumnFlags.None, 0, (int)LeaderboardListSorting.Rank);
			ImGui.TableSetupColumn("Players", ImGuiTableColumnFlags.None, 0, (int)LeaderboardListSorting.Players);
			ImGui.TableSetupColumn("World record", ImGuiTableColumnFlags.None, 0, (int)LeaderboardListSorting.WorldRecord);
			ImGui.TableHeadersRow();

			ImGuiTableSortSpecsPtr sortsSpecs = ImGui.TableGetSortSpecs();
			if (sortsSpecs.NativePtr != (void*)0 && sortsSpecs.SpecsDirty)
			{
				LeaderboardListChild.Sorting = (LeaderboardListSorting)sortsSpecs.Specs.ColumnUserID;
				LeaderboardListChild.SortAscending = sortsSpecs.Specs.SortDirection == ImGuiSortDirection.Ascending;
				LeaderboardListChild.UpdatePagedCustomLeaderboards();

				sortsSpecs.SpecsDirty = false;
			}

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
