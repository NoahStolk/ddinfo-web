using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.App.Ui.Base.Styling;
using DevilDaggersInfo.Common;
using DevilDaggersInfo.Core.Wiki;
using System.Diagnostics;
using Warp.NET.Content;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Recording;

public class RecordingResultHighscore : AbstractComponent
{
	private const int _yStart = 24;
	private const int _labelHeight = 16;

	public RecordingResultHighscore(IBounds bounds, GetUploadResponseHighscore response, bool isAscending)
		: base(bounds)
	{
		Label header = new(Bounds.CreateNested(0, 0, Bounds.Size.X, 16), "NEW HIGHSCORE!", LabelStyles.DefaultLeft) { Depth = Depth + 2 };
		NestingContext.Add(header);

		int y = _yStart;
		Add("Rank", response.RankState, i => i.ToString(), i => $"{i:+0;-0;+0}");

		AddSpacing(ref y);
		AddIcon(ref y, WarpTextures.IconEye, Color.Orange);
		Add("Time", response.TimeState, d => d.ToString(StringFormats.TimeFormat), i => $"{i:+0.0000;-0.0000;+0.0000}", !isAscending);
		Add("Level up 2", response.LevelUpTime2State, i => i.ToString(StringFormats.TimeFormat), i => $"{i:+0.0000;-0.0000;+0.0000}", false); // TODO: When value is 0, show "N/A" instead of "0.0000". TODO: When value difference is the same as the previous value, show "N/A".
		Add("Level up 3", response.LevelUpTime3State, i => i.ToString(StringFormats.TimeFormat), i => $"{i:+0.0000;-0.0000;+0.0000}", false);
		Add("Level up 4", response.LevelUpTime4State, i => i.ToString(StringFormats.TimeFormat), i => $"{i:+0.0000;-0.0000;+0.0000}", false);
		AddDeath(ref y);

		AddSpacing(ref y);
		AddIcon(ref y, WarpTextures.IconGem, Color.Red);
		Add("Gems collected", response.GemsCollectedState, i => i.ToString(), i => $"{i:+0;-0;+0}");
		Add("Gems despawned", response.GemsDespawnedState, i => i.ToString(), i => $"{i:+0;-0;+0}", false);
		Add("Gems eaten", response.GemsEatenState, i => i.ToString(), i => $"{i:+0;-0;+0}", false);
		Add("Gems total", response.GemsTotalState, i => i.ToString(), i => $"{i:+0;-0;+0}");

		AddSpacing(ref y);
		AddIcon(ref y, WarpTextures.IconHoming, Color.White);
		Add("Homing stored", response.HomingStoredState, i => i.ToString(), i => $"{i:+0;-0;+0}");
		Add("Homing eaten", response.HomingEatenState, i => i.ToString(), i => $"{i:+0;-0;+0}", false);

		AddSpacing(ref y);
		AddIcon(ref y, WarpTextures.IconCrosshair, Color.Green);
		Add("Daggers fired", response.DaggersFiredState, i => i.ToString(), i => $"{i:+0;-0;+0}");
		Add("Daggers hit", response.DaggersHitState, i => i.ToString(), i => $"{i:+0;-0;+0}");

		GetScoreState<double> accuracy = new()
		{
			Value = response.DaggersFiredState.Value == 0 ? 0 : response.DaggersHitState.Value / (double)response.DaggersFiredState.Value,
			ValueDifference = response.DaggersFiredState.ValueDifference == 0 ? 0 : response.DaggersHitState.ValueDifference / (double)response.DaggersFiredState.ValueDifference,
		};
		Add("Accuracy", accuracy, i => i.ToString(StringFormats.AccuracyFormat), i => $"{i:+0.00%;-0.00%;+0.00%}");

		AddSpacing(ref y);
		AddIcon(ref y, WarpTextures.IconSkull, EnemiesV3_2.Skull4.Color.ToWarpColor());
		Add("Enemies killed", response.EnemiesKilledState, i => i.ToString(), i => $"{i:+0;-0;+0}");
		Add("Enemies alive", response.EnemiesAliveState, i => i.ToString(), i => $"{i:+0;-0;+0}");

		void Add<T>(string label, GetScoreState<T> scoreState, Func<T, string> formatter, Func<T, string> formatterDifference, bool higherIsBetter = true)
			where T : struct, INumber<T>
		{
			int comparison = scoreState.ValueDifference.CompareTo(T.Zero);
			if (!higherIsBetter)
				comparison = -comparison;
			Color color = comparison switch
			{
				-1 => Color.Red,
				0 => Color.White,
				1 => Color.Green,
				_ => throw new UnreachableException(),
			};

			int columnWidth = Bounds.Size.X / 3;

			Label l = new(Bounds.CreateNested(columnWidth * 0, y, columnWidth, 16), label, LabelStyles.DefaultLeft) { Depth = Depth + 2 };
			Label m = new(Bounds.CreateNested(columnWidth * 1, y, columnWidth, 16), formatter(scoreState.Value), LabelStyles.DefaultRight) { Depth = Depth + 2 };
			Label r = new(Bounds.CreateNested(columnWidth * 2, y, columnWidth, 16), formatterDifference(scoreState.ValueDifference), LabelStyles.DefaultRight with { TextColor = color }) { Depth = Depth + 2 };
			NestingContext.Add(l);
			NestingContext.Add(m);
			NestingContext.Add(r);

			y += _labelHeight;
		}
	}

	private static void AddSpacing(ref int y)
	{
		y += _labelHeight / 2;
	}

	private void AddIcon(ref int y, Texture texture, Color color)
	{
		const int iconSize = 16;
		RecordingIcon recordingIcon = new(Bounds.CreateNested(4, y, iconSize, iconSize), texture, color) { Depth = Depth + 100 };
		NestingContext.Add(recordingIcon);

		y += iconSize;
	}

	private void AddDeath(ref int y)
	{
		Label left = new(Bounds.CreateNested(0, y, Bounds.Size.X / 2, 16), "Death", LabelStyles.DefaultLeft) { Depth = Depth + 2 };
		Label right = new(Bounds.CreateNested(Bounds.Size.X / 2, y, Bounds.Size.X / 2, 16), Deaths.GetDeathByLeaderboardType(GameConstants.CurrentVersion, Root.Dependencies.GameMemoryService.MainBlock.DeathType)?.Name ?? "?", LabelStyles.DefaultRight) { Depth = Depth + 2 };
		NestingContext.Add(left);
		NestingContext.Add(right);

		y += _labelHeight;
	}
}
