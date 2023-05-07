using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Ui.Base.Networking;
using DevilDaggersInfo.App.Ui.Base.Networking.TaskHandlers;
using DevilDaggersInfo.App.Ui.CustomLeaderboards.Leaderboard;
using DevilDaggersInfo.Common;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboards.LeaderboardList;

public static class LeaderboardListViewChild
{
	public static void Render()
	{
		ImGui.BeginChild("LeaderboardList");

		if (LeaderboardListChild.IsLoading)
		{
			ImGui.TextColored(Color.Yellow, "Loading...");
		}
		else
		{
			ImGui.Text($"Page {LeaderboardListChild.PageIndex + 1} of {LeaderboardListChild.TotalPages}");

			RenderTable();
		}

		ImGui.EndChild();
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
					AsyncHandler.Run(
						l =>
						{
							if (l == null)
							{
								Modals.ShowError = true;
								Modals.ErrorText = "Could not fetch custom leaderboard.";
								LeaderboardChild.Data = null;
							}
							else
							{
								LeaderboardChild.Data = new(l, lb.SpawnsetId);
							}
						},
						() => FetchCustomLeaderboardById.HandleAsync(lb.Id));
				}

				ImGui.TableNextColumn();

				ImGui.Text(lb.SpawnsetAuthorName);
				ImGui.TableNextColumn();

				ImGui.Text(lb.Criteria.Count.ToString());
				ImGui.TableNextColumn();

				ImGui.TextColored(CustomLeaderboardDaggerUtils.GetColor(lb.SelectedPlayerStats?.Dagger), lb.SelectedPlayerStats?.Time.ToString(StringFormats.TimeFormat) ?? "-");
				ImGui.TableNextColumn();

				bool completed = lb.SelectedPlayerStats?.Dagger == CustomLeaderboardDagger.Leviathan;
				Color color = CustomLeaderboardDaggerUtils.GetColor(completed ? CustomLeaderboardDagger.Leviathan : lb.SelectedPlayerStats?.NextDagger?.Dagger);
				ImGui.TextColored(color, completed ? "COMPLETED" : lb.SelectedPlayerStats?.NextDagger?.Time.ToString(StringFormats.TimeFormat) ?? "N/A");
				ImGui.TableNextColumn();

				ImGui.Text(lb.SelectedPlayerStats?.Rank.ToString() ?? "-");
				ImGui.TableNextColumn();

				ImGui.Text(lb.PlayerCount.ToString());
				ImGui.TableNextColumn();

				ImGui.TextColored(CustomLeaderboardDaggerUtils.GetColor(lb.WorldRecord?.Dagger), lb.WorldRecord?.Time.ToString(StringFormats.TimeFormat) ?? "-");
				ImGui.TableNextColumn();
			}

			ImGui.PopStyleVar();
			ImGui.EndTable();
		}
	}
}
