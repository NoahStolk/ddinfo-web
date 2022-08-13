using DevilDaggersInfo.App.Core.GameMemory;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecordingFeature.Actions;

public record SetRecordingAction(MainBlock Block, MainBlock BlockPrevious);
