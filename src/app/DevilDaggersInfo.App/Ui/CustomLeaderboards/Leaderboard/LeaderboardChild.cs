using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.Api.App.Spawnsets;
using DevilDaggersInfo.App.Ui.Base.Networking;
using DevilDaggersInfo.App.Ui.Base.Networking.TaskHandlers;
using DevilDaggersInfo.App.Ui.Base.User.Settings;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboards.Leaderboard;

public static class LeaderboardChild
{
	public static LeaderboardData? Data { get; set; }

	public static void Render()
	{
		ImGui.BeginChild("LeaderboardChild");

		if (Data == null)
			ImGui.Text("None selected");
		else
			RenderLeaderboard(Data);

		ImGui.EndChild();
	}

	private static void RenderLeaderboard(LeaderboardData data)
	{
		ImGui.Text(data.Leaderboard.SpawnsetName);

		if (ImGui.Button("Play", new(80, 20)))
		{
			AsyncHandler.Run(InstallSpawnset, () => FetchSpawnsetById.HandleAsync(data.SpawnsetId));
			void InstallSpawnset(GetSpawnset? spawnset)
			{
				if (spawnset == null)
				{
					Modals.ShowError = true;
					Modals.ErrorText = "Could not fetch spawnset.";
					return;
				}

				File.WriteAllBytes(UserSettings.ModsSurvivalPath, spawnset.FileBytes);
			}
		}
	}

	public sealed record LeaderboardData(GetCustomLeaderboard Leaderboard, int SpawnsetId);
}
