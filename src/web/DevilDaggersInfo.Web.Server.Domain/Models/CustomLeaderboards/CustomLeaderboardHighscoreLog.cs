namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomLeaderboardHighscoreLog(CustomLeaderboardDagger Dagger, int CustomLeaderboardId, string Message, int Rank, int TotalPlayers, int Time);
