using DevilDaggersInfo.App.Core.GameMemory;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;

public record RecordingState(bool ShowEnemyStats, MainBlock Block, MainBlock BlockPrevious);
