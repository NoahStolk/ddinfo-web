using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;
using DevilDaggersInfo.Common;
using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Objects;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Leaderboard;

public class LeaderboardEntry : AbstractComponent
{
	public const int Height = 16;

	private readonly GetCustomEntry _getCustomEntry;

	private readonly float _accuracy;
	private readonly Death? _death;
	private readonly double? _level2;
	private readonly double? _level3;
	private readonly double? _level4;

	private bool _isHovering;

	public LeaderboardEntry(IBounds bounds, GetCustomEntry getCustomEntry)
		: base(bounds)
	{
		_getCustomEntry = getCustomEntry;

		_accuracy = _getCustomEntry.DaggersFired == 0 ? 0 : _getCustomEntry.DaggersHit / (float)_getCustomEntry.DaggersFired;
		_death = Deaths.GetDeathByLeaderboardType(GameConstants.CurrentVersion, _getCustomEntry.DeathType);
		_level2 = _getCustomEntry.LevelUpTime2InSeconds == 0 ? null : _getCustomEntry.LevelUpTime2InSeconds;
		_level3 = _getCustomEntry.LevelUpTime3InSeconds == 0 ? null : _getCustomEntry.LevelUpTime3InSeconds;
		_level4 = _getCustomEntry.LevelUpTime4InSeconds == 0 ? null : _getCustomEntry.LevelUpTime4InSeconds;

		TooltipIconButton watchInGame = new(Bounds.CreateNested(160, 0, 16, 16), WatchInGame, GlobalStyles.DefaultButtonStyle, WarpTextures.IconEye, "Watch in-game") { IsDisabled = !_getCustomEntry.HasReplay };
		NestingContext.Add(watchInGame);

		TooltipIconButton watchInReplayViewer = new(Bounds.CreateNested(176, 0, 16, 16), WatchInReplayViewer, GlobalStyles.DefaultButtonStyle, WarpTextures.IconEye, "Watch in replay viewer") { IsDisabled = !_getCustomEntry.HasReplay };
		NestingContext.Add(watchInReplayViewer);
	}

	private void WatchInGame()
	{
		AsyncHandler.Run(Inject, () => FetchCustomEntryReplayById.HandleAsync(_getCustomEntry.Id));

		void Inject(GetCustomEntryReplayBuffer? getCustomEntryReplayBuffer)
		{
			if (getCustomEntryReplayBuffer == null)
			{
				// TODO: Show error.
				return;
			}

			Root.Game.GameMemoryService.WriteReplayToMemory(getCustomEntryReplayBuffer.Data);
		}
	}

	private void WatchInReplayViewer()
	{
		// TEMP:
		// ReplayBinary<LocalReplayBinaryHeader> replayBinary = new(File.ReadAllBytes("""C:\Users\NOAH\AppData\Roaming\DevilDaggers\replays\jumps2_10.16-xvlv-0cd13c38[cdb2acd5].ddreplay"""));
		// LayoutManager.ToCustomLeaderboardsRecorderReplayViewer3dLayout(new[]{replayBinary});

		AsyncHandler.Run(BuildReplayScene, () => FetchCustomEntryReplayById.HandleAsync(_getCustomEntry.Id));

		void BuildReplayScene(GetCustomEntryReplayBuffer? getCustomEntryReplayBuffer)
		{
			if (getCustomEntryReplayBuffer == null)
			{
				// TODO: Show error.
				return;
			}

			ReplayBinary<LocalReplayBinaryHeader> replayBinary;
			try
			{
				replayBinary = new(getCustomEntryReplayBuffer.Data);
			}
			catch (Exception ex)
			{
				// TODO: Show error.
				return;
			}

			LayoutManager.ToCustomLeaderboardsRecorderReplayViewer3dLayout(new[] { replayBinary });
		}
	}

	public override void Update(Vector2i<int> scrollOffset)
	{
		base.Update(scrollOffset);

		_isHovering = MouseUiContext.Contains(scrollOffset, Bounds);
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		bool isCurrentPlayer = _getCustomEntry.PlayerId == StateManager.RecordingState.CurrentPlayerId;
		if (isCurrentPlayer || _isHovering)
		{
			Color color = !_getCustomEntry.HasReplay ? new(63, 0, 0, 127) : (isCurrentPlayer, _isHovering) switch
			{
				(true, true) => GlobalColors.EntrySelectHover,
				(true, false) => GlobalColors.EntryHover,
				(false, true) => GlobalColors.EntrySelect,
				_ => throw new InvalidOperationException(),
			};
			Root.Game.RectangleRenderer.Schedule(Bounds.Size, Bounds.Center + scrollOffset, Depth - 1, color);
		}

		Vector2i<int> position = Bounds.TopLeft + new Vector2i<int>(0, 4) + scrollOffset;
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(LeaderboardWrapper.TableOffsets[00], 0), Depth, Color.White, _getCustomEntry.Rank.ToString("00"), TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(LeaderboardWrapper.TableOffsets[01], 0), Depth, Color.White, _getCustomEntry.PlayerName, TextAlign.Left);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(LeaderboardWrapper.TableOffsets[02], 0), Depth, _getCustomEntry.CustomLeaderboardDagger?.GetColor() ?? Color.White, _getCustomEntry.TimeInSeconds.ToString(StringFormats.TimeFormat), TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(LeaderboardWrapper.TableOffsets[03], 0), Depth, Color.White, _getCustomEntry.EnemiesAlive.ToString(), TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(LeaderboardWrapper.TableOffsets[04], 0), Depth, Color.White, _getCustomEntry.EnemiesKilled.ToString(), TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(LeaderboardWrapper.TableOffsets[05], 0), Depth, Color.White, _getCustomEntry.GemsCollected.ToString(), TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(LeaderboardWrapper.TableOffsets[06], 0), Depth, Color.White, _getCustomEntry.GemsDespawned?.ToString() ?? "-", TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(LeaderboardWrapper.TableOffsets[07], 0), Depth, Color.White, _getCustomEntry.GemsEaten?.ToString() ?? "-", TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(LeaderboardWrapper.TableOffsets[08], 0), Depth, Color.White, _accuracy.ToString(StringFormats.AccuracyFormat), TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(LeaderboardWrapper.TableOffsets[09], 0), Depth, _death?.Color.ToWarpColor() ?? Color.White, _death?.Name ?? "-", TextAlign.Left);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(LeaderboardWrapper.TableOffsets[10], 0), Depth, Color.White, _getCustomEntry.HomingStored.ToString(), TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(LeaderboardWrapper.TableOffsets[11], 0), Depth, Color.White, _getCustomEntry.HomingEaten?.ToString() ?? "-", TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(LeaderboardWrapper.TableOffsets[12], 0), Depth, Color.White, _level2?.ToString(StringFormats.TimeFormat) ?? "-", TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(LeaderboardWrapper.TableOffsets[13], 0), Depth, Color.White, _level3?.ToString(StringFormats.TimeFormat) ?? "-", TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(LeaderboardWrapper.TableOffsets[14], 0), Depth, Color.White, _level4?.ToString(StringFormats.TimeFormat) ?? "-", TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(LeaderboardWrapper.TableOffsets[15], 0), Depth, Color.White, _getCustomEntry.SubmitDate.ToString(StringFormats.DateTimeFormat), TextAlign.Right);
	}
}
