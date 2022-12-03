using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.Common;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Objects;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;

public class RecordingWrapper : AbstractComponent
{
	private readonly RecordingValue _player;
	private readonly RecordingValue _status;
	private readonly RecordingValue _time;
	private readonly RecordingValue _deathType;
	private readonly RecordingValue _gems;
	private readonly RecordingValue _homing;
	private readonly RecordingValue _kills;

	public RecordingWrapper(IBounds bounds)
		: base(bounds)
	{
		const int labelWidth = 256;
		const int labelHeight = 16;

		_player = new(bounds.CreateNested(0, labelHeight * 0, labelWidth, labelHeight), "Player", Color.White) { Depth = Depth };
		_status = new(bounds.CreateNested(0, labelHeight * 1, labelWidth, labelHeight), "Status", Color.White) { Depth = Depth };
		_time = new(bounds.CreateNested(0, labelHeight * 2, labelWidth, labelHeight), "Time", Color.White) { Depth = Depth };
		_deathType = new(bounds.CreateNested(0, labelHeight * 3, labelWidth, labelHeight), "Death", Color.White) { Depth = Depth };

		_gems = new(bounds.CreateNested(0, labelHeight * 5, labelWidth, labelHeight), "Gems", Color.Red) { Depth = Depth };
		_homing = new(bounds.CreateNested(0, labelHeight * 6, labelWidth, labelHeight), "Homing", Color.Purple) { Depth = Depth };

		_kills = new(bounds.CreateNested(0, labelHeight * 8, labelWidth, labelHeight), "Kills", Color.Orange) { Depth = Depth };

		NestingContext.Add(_player);
		NestingContext.Add(_status);
		NestingContext.Add(_time);
		NestingContext.Add(_deathType);
		NestingContext.Add(_gems);
		NestingContext.Add(_homing);
		NestingContext.Add(_kills);
	}

	public void SetState()
	{
		MainBlock block = Root.Game.GameMemoryService.MainBlock;
		GameStatus gameStatus = (GameStatus)block.Status;
		Death? death = Deaths.GetDeathByLeaderboardType(GameConstants.CurrentVersion, block.DeathType);

		_player.UpdateValue($"{block.PlayerName} ({block.PlayerId})");
		_status.UpdateValue(gameStatus.ToString());
		_time.UpdateValue(block.Time.ToString(StringFormats.TimeFormat));
		_deathType.UpdateValue(block.IsPlayerAlive ? "Alive" : death?.Name ?? "?", death?.Color.ToWarpColor());

		_gems.UpdateValue(block.GemsCollected.ToString());
		_homing.UpdateValue(block.HomingStored.ToString());

		_kills.UpdateValue(block.EnemiesKilled.ToString());
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		const int border = 1;
		Root.Game.RectangleRenderer.Schedule(Bounds.Size, Bounds.Center + scrollOffset, Depth, Color.Purple);
		Root.Game.RectangleRenderer.Schedule(Bounds.Size - new Vector2i<int>(border * 2), Bounds.Center + scrollOffset, 1, Color.Black);
	}
}
