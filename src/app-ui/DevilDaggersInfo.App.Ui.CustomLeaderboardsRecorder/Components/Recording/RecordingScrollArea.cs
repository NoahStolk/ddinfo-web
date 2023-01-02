using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.App.Core.GameMemory.Extensions;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.App.Ui.Base.Styling;
using DevilDaggersInfo.Common;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Objects;
using Warp.NET.Content;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Recording;

public class RecordingScrollArea : ScrollArea
{
	private readonly List<RecordingValue> _recordingValues = new();

	public RecordingScrollArea(IBounds bounds)
		: base(bounds, 64, 16, ScrollAreaStyles.Default)
	{
		int labelWidth = 256 - ScrollbarBounds.Size.X;
		const int labelHeight = 16;

		int height = 0;

		_recordingValues.Add(AddValue(ref height, "Status", Color.White, b => ((GameStatus)b.Status).ToDisplayString()));

		AddSpacing(ref height);
		AddIcon(ref height, WarpTextures.IconEye, Color.Orange);
		_recordingValues.Add(AddValue(ref height, "Player", Color.White, b => $"{b.PlayerName} ({b.PlayerId})"));
		_recordingValues.Add(AddValue(ref height, "Time", Color.White, b => b.Time.ToString(StringFormats.TimeFormat)));
		_recordingValues.Add(AddValue(ref height, "Hand", Color.White, b => GetUpgrade(b).Name, b => GetUpgrade(b).Color.ToWarpColor()));
		_recordingValues.Add(AddValue(ref height, "Level 2", Color.White, b => b.LevelUpTime2 == 0 ? "-" : b.LevelUpTime2.ToString(StringFormats.TimeFormat), _ => UpgradesV3_2.Level2.Color.ToWarpColor()));
		_recordingValues.Add(AddValue(ref height, "Level 3", Color.White, b => b.LevelUpTime3 == 0 ? "-" : b.LevelUpTime3.ToString(StringFormats.TimeFormat), _ => UpgradesV3_2.Level3.Color.ToWarpColor()));
		_recordingValues.Add(AddValue(ref height, "Level 4", Color.White, b => b.LevelUpTime4 == 0 ? "-" : b.LevelUpTime4.ToString(StringFormats.TimeFormat), _ => UpgradesV3_2.Level4.Color.ToWarpColor()));
		_recordingValues.Add(AddValue(ref height, "Death", Color.White, b => b.IsPlayerAlive ? "Alive" : GetDeath(b)?.Name ?? "?", b => GetDeath(b)?.Color.ToWarpColor() ?? Color.White));

		AddSpacing(ref height);
		AddIcon(ref height, WarpTextures.IconGem, Color.Red);
		_recordingValues.Add(AddValue(ref height, "Gems collected", Color.Red, b => b.GemsCollected.ToString()));
		_recordingValues.Add(AddValue(ref height, "Gems despawned", Color.Gray(0.6f), b => b.GemsDespawned.ToString()));
		_recordingValues.Add(AddValue(ref height, "Gems eaten", Color.Green, b => b.GemsEaten.ToString()));
		_recordingValues.Add(AddValue(ref height, "Gems total", Color.Red, b => b.GemsTotal.ToString()));

		AddSpacing(ref height);
		AddIcon(ref height, WarpTextures.IconHoming, Color.White);
		_recordingValues.Add(AddValue(ref height, "Homing stored", Color.Purple, b => b.HomingStored.ToString()));
		_recordingValues.Add(AddValue(ref height, "Homing eaten", Color.Red, b => b.HomingEaten.ToString()));

		AddSpacing(ref height);
		AddIcon(ref height, WarpTextures.IconDagger, Color.Blue);
		_recordingValues.Add(AddValue(ref height, "Daggers fired", Color.Yellow, b => b.DaggersFired.ToString()));
		_recordingValues.Add(AddValue(ref height, "Daggers hit", Color.Red, b => b.DaggersHit.ToString()));
		_recordingValues.Add(AddValue(ref height, "Accuracy", Color.Orange, b => (b.DaggersFired == 0 ? 0 : b.DaggersHit / (float)b.DaggersFired).ToString("0.00%")));

		AddSpacing(ref height);
		AddIcon(ref height, WarpTextures.IconSkull, EnemiesV3_2.Skull4.Color.ToWarpColor());
		_recordingValues.Add(AddValue(ref height, "Enemies killed", Color.Red, b => b.EnemiesKilled.ToString()));
		_recordingValues.Add(AddValue(ref height, "Enemies alive", Color.Yellow, b => b.EnemiesAlive.ToString()));

		RecordingValue AddValue(ref int h, string text, Color color, Func<MainBlock, string> valueGetter, Func<MainBlock, Color>? colorGetter = null)
		{
			RecordingValue recordingValue = new(bounds.CreateNested(0, h, labelWidth, labelHeight), text, color, valueGetter, colorGetter) { Depth = Depth };
			NestingContext.Add(recordingValue);

			h += labelHeight;

			return recordingValue;
		}

		void AddSpacing(ref int h)
		{
			h += labelHeight / 2;
		}

		void AddIcon(ref int h, Texture texture, Color color)
		{
			const int iconSize = 16;
			RecordingIcon recordingIcon = new(bounds.CreateNested(4, h, iconSize, iconSize), texture, color) { Depth = Depth + 100 };
			NestingContext.Add(recordingIcon);

			h += iconSize;
		}
	}

	private static bool ShouldRenderRecording => Root.Dependencies.GameMemoryService.IsInitialized && (GameStatus)Root.Dependencies.GameMemoryService.MainBlock.Status is not (GameStatus.Title or GameStatus.Menu or GameStatus.Lobby);

	public void SetState()
	{
		if (!ShouldRenderRecording)
			return;

		foreach (RecordingValue recordingValue in _recordingValues)
			recordingValue.SetState();
	}

	private static Upgrade GetUpgrade(MainBlock block) => block.LevelGems switch
	{
		< 10 => UpgradesV3_2.Level1,
		< 70 => UpgradesV3_2.Level2,
		70 => UpgradesV3_2.Level3,
		_ => UpgradesV3_2.Level4,
	};

	private static Death? GetDeath(MainBlock block)
	{
		return Deaths.GetDeathByLeaderboardType(GameConstants.CurrentVersion, block.DeathType);
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		if (ShouldRenderRecording)
			base.Render(scrollOffset);

		const int border = 1;
		Root.Game.RectangleRenderer.Schedule(ContentBounds.Size, ContentBounds.Center + scrollOffset, Depth, Color.Purple);
		Root.Game.RectangleRenderer.Schedule(ContentBounds.Size - new Vector2i<int>(border * 2), ContentBounds.Center + scrollOffset, 1, Color.Black);
	}
}
