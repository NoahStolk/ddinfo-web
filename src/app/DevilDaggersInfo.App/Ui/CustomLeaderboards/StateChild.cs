using DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Data;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboards;

public static class StateChild
{
	public static ulong? Marker { get; set; }
	public static RecordingStateType RecordingState { get; set; }
	public static string? SpawnsetName { get; set; }
	public static DateTime? LastSubmission { get; set; }

	public static void Render()
	{
		if (ImGui.BeginTable("StateTable", 2, ImGuiTableFlags.None, new(320, 160)))
		{
			ImGui.TableNextColumn();
			ImGui.Text("Memory");
			ImGui.TableNextColumn();
			ImGui.Text(Marker.HasValue ? $"0x{Marker.Value:X}" : "Waiting...");
			ImGui.TableNextRow();

			ImGui.TableNextColumn();
			ImGui.Text("State");
			ImGui.TableNextColumn();
			ImGui.Text(RecordingState.ToDisplayString());
			ImGui.TableNextRow();

			ImGui.TableNextColumn();
			ImGui.Text("Spawnset");
			ImGui.TableNextColumn();
			ImGui.Text(SpawnsetName ?? "(unknown)");
			ImGui.TableNextRow();

			ImGui.TableNextColumn();
			ImGui.Text("Last upload");
			ImGui.TableNextColumn();
			ImGui.Text(DateTimeUtils.FormatTimeAgo(LastSubmission));
			ImGui.TableNextRow();

			ImGui.EndTable();
		}
	}
}
