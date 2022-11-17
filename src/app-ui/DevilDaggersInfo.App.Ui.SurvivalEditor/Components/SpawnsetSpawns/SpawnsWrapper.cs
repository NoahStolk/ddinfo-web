using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;

public sealed class SpawnsWrapper : AbstractScrollViewer<SpawnsWrapper, Spawns>
{
	public SpawnsWrapper(IBounds bounds)
		: base(bounds)
	{
		Label title = new(Rectangle.At(0, 0, bounds.Size.X, 48), Color.White, "Spawns", TextAlign.Middle, FontSize.F12X12);
		NestingContext.Add(title);

		Rectangle spawnsMetric = Rectangle.At(0, 48, bounds.Size.X, bounds.Size.Y - 48);

		Content = new(spawnsMetric, this);
		Scrollbar = new(spawnsMetric with { X1 = spawnsMetric.X2, X2 = spawnsMetric.X2 + 16 }, SetScrollPercentage);

		NestingContext.Add(Content);
		NestingContext.Add(Scrollbar);
	}

	public override Scrollbar Scrollbar { get; }
	public override Spawns Content { get; }

	public override void InitializeContent()
	{
		int oldHeight = Content.ContentHeightInPixels;
		Content.SetSpawnset();
		int newHeight = Content.ContentHeightInPixels;

		SetThumbPercentageSize();

		if (oldHeight == 0)
		{
			SetScrollPercentage(0);
		}
		else
		{
			float multiplier = oldHeight / (float)newHeight;
			float newPercentage = Scrollbar.TopPercentage * multiplier;
			SetScrollPercentage(newPercentage);
		}
	}
}
