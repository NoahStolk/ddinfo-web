using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Ui.Base.Networking;
using DevilDaggersInfo.App.Ui.Base.Networking.TaskHandlers;
using DevilDaggersInfo.App.Ui.CustomLeaderboards.Leaderboard;
using DevilDaggersInfo.Common;
using DevilDaggersInfo.Core.CriteriaExpression;
using DevilDaggersInfo.Core.CriteriaExpression.Extensions;
using DevilDaggersInfo.Core.CriteriaExpression.Parts;
using ImGuiNET;
using System.Numerics;
using System.Text;

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
				foreach (GetCustomLeaderboardCriteria criteria in lb.Criteria)
				{
					ImGui.SameLine();
					ImGui.Image((IntPtr)criteria.Type.GetTexture().Handle, new(13));
					if (ImGui.IsItemHovered())
					{
						// TODO: May need to improve performance here by caching the text, or perhaps return the text from the API.
						ImGui.SetTooltip(GetText(criteria));
					}
				}

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

	private static string GetText(GetCustomLeaderboardCriteria criteria)
	{
		if (!Expression.TryParse(criteria.Expression, out Expression? criteriaExpression))
		{
			// TODO: Log warning.
			return string.Empty;
		}

		DevilDaggersInfo.Core.CriteriaExpression.CustomLeaderboardCriteriaType criteriaType = criteria.Type.ToCore();
		DevilDaggersInfo.Core.CriteriaExpression.CustomLeaderboardCriteriaOperator @operator = criteria.Operator.ToCore();

		StringBuilder sb = new();
		sb.Append(criteriaType.Display());
		sb.Append(' ');
		sb.Append(@operator.ShortString());
		sb.Append(' ');

		foreach (IExpressionPart expressionPart in criteriaExpression.Parts)
		{
			switch (expressionPart)
			{
				case ExpressionOperator op:
					sb.Append(op);
					break;
				case ExpressionTarget target:
					sb.Append(target);
					break;
				case ExpressionValue value:
					sb.Append(value.ToDisplayString(criteriaType));
					break;
			}

			sb.Append(' ');
		}

		return sb.ToString().Trim();
	}
}
