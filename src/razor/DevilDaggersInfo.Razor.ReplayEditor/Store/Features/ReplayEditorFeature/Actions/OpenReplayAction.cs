using DevilDaggersInfo.Core.Replay;

namespace DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayEditorFeature.Actions;

public record OpenReplayAction(ReplayBinary ReplayBinary, string Name);
