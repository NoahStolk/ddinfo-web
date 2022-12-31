using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.App.Core.GameMemory.Extensions;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.Common;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Objects;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Recording;

public class RecordingWrapper : AbstractComponent
{
	private readonly RecordingValue _status;

	private readonly RecordingValue _player;
	private readonly RecordingValue _time;
	private readonly RecordingValue _hand;
	private readonly RecordingValue _levelUpTime2;
	private readonly RecordingValue _levelUpTime3;
	private readonly RecordingValue _levelUpTime4;
	private readonly RecordingValue _deathType;

	private readonly RecordingValue _gemsCollected;
	private readonly RecordingValue _gemsDespawned;
	private readonly RecordingValue _gemsEaten;
	private readonly RecordingValue _gemsTotal;

	private readonly RecordingValue _homingStored;
	private readonly RecordingValue _homingEaten;

	private readonly RecordingValue _enemiesKilled;
	private readonly RecordingValue _enemiesAlive;

	private readonly RecordingValue _accuracy;

	public RecordingWrapper(IBounds bounds)
		: base(bounds)
	{
		const int labelWidth = 256;
		const int labelHeight = 16;

		int height = 0;

		_status = AddValue(ref height, "Status");

		AddSpacing(ref height);
		_player = AddValue(ref height, "Player");
		_time = AddValue(ref height, "Time");
		_hand = AddValue(ref height, "Hand");
		_levelUpTime2 = AddValue(ref height, "Level 2");
		_levelUpTime3 = AddValue(ref height, "Level 3");
		_levelUpTime4 = AddValue(ref height, "Level 4");
		_deathType = AddValue(ref height, "Death");

		AddSpacing(ref height);
		_gemsCollected = AddValue(ref height, "Gems collected", Color.Red);
		_gemsDespawned = AddValue(ref height, "Gems despawned", Color.Gray(0.6f));
		_gemsEaten = AddValue(ref height, "Gems eaten", Color.Green);
		_gemsTotal = AddValue(ref height, "Gems total", Color.Red);

		AddSpacing(ref height);
		_homingStored = AddValue(ref height, "Homing stored", Color.Purple);
		_homingEaten = AddValue(ref height, "Homing eaten", Color.Red);

		AddSpacing(ref height);
		_enemiesKilled = AddValue(ref height, "Enemies killed", Color.Red);
		_enemiesAlive = AddValue(ref height, "Enemies alive", Color.Yellow);

		AddSpacing(ref height);
		_accuracy = AddValue(ref height, "Accuracy", Color.Orange);

		RecordingValue AddValue(ref int h, string text, Color? color = null)
		{
			RecordingValue recordingValue = new(bounds.CreateNested(0, h, labelWidth, labelHeight), text, color ?? Color.White) { Depth = Depth };
			NestingContext.Add(recordingValue);

			h += labelHeight;

			return recordingValue;
		}

		void AddSpacing(ref int h)
		{
			h += labelHeight / 2;
		}
	}

	private static bool ShouldRenderRecording => Root.Dependencies.GameMemoryService.IsInitialized && (GameStatus)Root.Dependencies.GameMemoryService.MainBlock.Status is not (GameStatus.Title or GameStatus.Menu or GameStatus.Lobby);

	public void SetState()
	{
		if (!ShouldRenderRecording)
			return;

		MainBlock block = Root.Dependencies.GameMemoryService.MainBlock;
		GameStatus gameStatus = (GameStatus)block.Status;
		Death? death = Deaths.GetDeathByLeaderboardType(GameConstants.CurrentVersion, block.DeathType);
		float accuracy = block.DaggersFired == 0 ? 0 : block.DaggersHit / (float)block.DaggersFired;
		Upgrade upgrade = block.LevelGems switch
		{
			< 10 => UpgradesV3_2.Level1,
			< 70 => UpgradesV3_2.Level2,
			70 => UpgradesV3_2.Level3,
			_ => UpgradesV3_2.Level4,
		};

		_status.UpdateValue(gameStatus.ToDisplayString());

		_player.UpdateValue($"{block.PlayerName} ({block.PlayerId})");
		_time.UpdateValue(block.Time.ToString(StringFormats.TimeFormat));
		_hand.UpdateValue(upgrade.Name, upgrade.Color.ToWarpColor());
		_levelUpTime2.UpdateValue(block.LevelUpTime2 == 0 ? "-" : block.LevelUpTime2.ToString(StringFormats.TimeFormat), UpgradesV3_2.Level2.Color.ToWarpColor());
		_levelUpTime3.UpdateValue(block.LevelUpTime3 == 0 ? "-" : block.LevelUpTime3.ToString(StringFormats.TimeFormat), UpgradesV3_2.Level3.Color.ToWarpColor());
		_levelUpTime4.UpdateValue(block.LevelUpTime4 == 0 ? "-" : block.LevelUpTime4.ToString(StringFormats.TimeFormat), UpgradesV3_2.Level4.Color.ToWarpColor());
		_deathType.UpdateValue(block.IsPlayerAlive ? "Alive" : death?.Name ?? "?", death?.Color.ToWarpColor());

		_gemsCollected.UpdateValue(block.GemsCollected.ToString());
		_gemsDespawned.UpdateValue(block.GemsDespawned.ToString());
		_gemsEaten.UpdateValue(block.GemsEaten.ToString());
		_gemsTotal.UpdateValue(block.GemsTotal.ToString());

		_homingStored.UpdateValue(block.HomingStored.ToString());
		_homingEaten.UpdateValue(block.HomingEaten.ToString());

		_enemiesKilled.UpdateValue(block.EnemiesKilled.ToString());
		_enemiesAlive.UpdateValue(block.EnemiesAlive.ToString());

		_accuracy.UpdateValue($"{block.DaggersHit} / {block.DaggersFired} ({accuracy:0.00%})");
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		if (ShouldRenderRecording)
			base.Render(scrollOffset);

		const int border = 1;
		Root.Game.RectangleRenderer.Schedule(Bounds.Size, Bounds.Center + scrollOffset, Depth, Color.Purple);
		Root.Game.RectangleRenderer.Schedule(Bounds.Size - new Vector2i<int>(border * 2), Bounds.Center + scrollOffset, 1, Color.Black);
	}
}
