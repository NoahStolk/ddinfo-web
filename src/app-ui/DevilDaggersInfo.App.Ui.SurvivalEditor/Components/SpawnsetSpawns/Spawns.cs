using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Spawns;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Silk.NET.OpenGL;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;

public class Spawns : AbstractScrollContent<Spawns, SpawnsWrapper>
{
	public const int SpawnEntryHeight = 24;

	private readonly List<AbstractComponent> _spawnComponents = new();

	public Spawns(Rectangle metric, SpawnsWrapper spawnsWrapper)
		: base(metric, spawnsWrapper)
	{
	}

	public override int ContentHeightInPixels => _spawnComponents.Count * SpawnEntryHeight;

	public override void Render(Vector2i<int> parentPosition)
	{
		Gl.Enable(EnableCap.ScissorTest);
		SetScissor(parentPosition);

		Root.Game.UiRenderer.RenderTopLeft(Metric.Size, parentPosition + new Vector2i<int>(Metric.X1, Metric.Y1), Depth, Color.Black);

		base.Render(parentPosition);

		Gl.Disable(EnableCap.ScissorTest);
	}

	public override void RenderText(Vector2i<int> parentPosition)
	{
		Gl.Enable(EnableCap.ScissorTest);
		SetScissor(parentPosition);

		base.RenderText(parentPosition);

		Gl.Disable(EnableCap.ScissorTest);
	}

	private void SetScissor(Vector2i<int> parentPosition)
	{
		Vector2 viewportOffset = Root.Game.ViewportOffset;
		Gl.Scissor(Metric.X1 + (int)viewportOffset.X + parentPosition.X, Root.Game.InitialWindowHeight - (Metric.Size.Y + parentPosition.Y) + (int)viewportOffset.Y, (uint)Metric.Size.X, (uint)Metric.Size.Y);
	}

	public void SetSpawnset()
	{
		foreach (AbstractComponent component in _spawnComponents)
			NestingContext.Remove(component);

		_spawnComponents.Clear();

		NestingContext.ScrollOffset = default;

		int i = 0;
		foreach (EditableSpawn spawn in EditSpawnContext.GetFrom(StateManager.SpawnsetState.Spawnset))
		{
			SpawnEntry spawnEntry = new(Rectangle.At(0, i++ * SpawnEntryHeight, 512, SpawnEntryHeight), spawn);
			_spawnComponents.Add(spawnEntry);
		}

		foreach (AbstractComponent component in _spawnComponents)
			NestingContext.Add(component);
	}
}
