using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.Common;
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

	public LeaderboardEntry(IBounds bounds, GetCustomEntry getCustomEntry)
		: base(bounds)
	{
		_getCustomEntry = getCustomEntry;
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		float accuracy = _getCustomEntry.DaggersHit / (float)_getCustomEntry.DaggersFired;
		Death? death = Deaths.GetDeathByLeaderboardType(GameConstants.CurrentVersion, _getCustomEntry.DeathType);
		double? level2 = _getCustomEntry.LevelUpTime2InSeconds == 0 ? null : _getCustomEntry.LevelUpTime2InSeconds;
		double? level3 = _getCustomEntry.LevelUpTime3InSeconds == 0 ? null : _getCustomEntry.LevelUpTime3InSeconds;
		double? level4 = _getCustomEntry.LevelUpTime4InSeconds == 0 ? null : _getCustomEntry.LevelUpTime4InSeconds;

		Vector2i<int> position = Bounds.TopLeft + scrollOffset;
		Span<int> offsets = stackalloc int[16] { 16, 24, 260, 308, 352, 400, 448, 496, 552, 560, 656, 712, 760, 832, 888, 992 };
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(offsets[00], 0), Depth, Color.White, _getCustomEntry.Rank.ToString("00"), TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(offsets[01], 0), Depth, Color.White, _getCustomEntry.PlayerName, TextAlign.Left);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(offsets[02], 0), Depth, _getCustomEntry.CustomLeaderboardDagger?.GetColor() ?? Color.White, _getCustomEntry.TimeInSeconds.ToString(StringFormats.TimeFormat), TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(offsets[03], 0), Depth, Color.White, _getCustomEntry.EnemiesAlive.ToString(), TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(offsets[04], 0), Depth, Color.White, _getCustomEntry.EnemiesKilled.ToString(), TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(offsets[05], 0), Depth, Color.White, _getCustomEntry.GemsCollected.ToString(), TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(offsets[06], 0), Depth, Color.White, _getCustomEntry.GemsDespawned?.ToString() ?? "-", TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(offsets[07], 0), Depth, Color.White, _getCustomEntry.GemsEaten?.ToString() ?? "-", TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(offsets[08], 0), Depth, Color.White, accuracy.ToString(StringFormats.AccuracyFormat), TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(offsets[09], 0), Depth, death?.Color.ToWarpColor() ?? Color.White, death?.Name ?? "-", TextAlign.Left);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(offsets[10], 0), Depth, Color.White, _getCustomEntry.HomingStored.ToString(), TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(offsets[11], 0), Depth, Color.White, _getCustomEntry.HomingEaten?.ToString() ?? "-", TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(offsets[12], 0), Depth, Color.White, level2?.ToString(StringFormats.TimeFormat) ?? "-", TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(offsets[13], 0), Depth, Color.White, level3?.ToString(StringFormats.TimeFormat) ?? "-", TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(offsets[14], 0), Depth, Color.White, level4?.ToString(StringFormats.TimeFormat) ?? "-", TextAlign.Right);
		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), position + new Vector2i<int>(offsets[15], 0), Depth, Color.White, _getCustomEntry.SubmitDate.ToString(StringFormats.DateTimeFormat), TextAlign.Right);
	}
}
