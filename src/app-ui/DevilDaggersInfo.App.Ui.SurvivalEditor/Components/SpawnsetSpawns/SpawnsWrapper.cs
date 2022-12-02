using DevilDaggersInfo.App.Ui.Base;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;

public class SpawnsWrapper : AbstractComponent
{
	private readonly ScrollViewer<Spawns> _spawnsViewer;

	public SpawnsWrapper(IBounds bounds)
		: base(bounds)
	{
		const int titleHeight = 48;

		Label title = new(bounds.CreateNested(0, 0, bounds.Size.X, titleHeight), "Spawns", GlobalStyles.LabelTitle);
		_spawnsViewer = new(bounds.CreateNested(0, titleHeight, bounds.Size.X, bounds.Size.Y - titleHeight), 16);

		NestingContext.Add(title);
		NestingContext.Add(_spawnsViewer);
	}

	public void SetSpawnset()
	{
		_spawnsViewer.InitializeContent();
	}
}
