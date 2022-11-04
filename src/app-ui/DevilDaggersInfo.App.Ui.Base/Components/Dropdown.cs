using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Enums;
using Silk.NET.GLFW;
using Warp;
using Warp.Extensions;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class Dropdown : AbstractDropdown
{
	private readonly int _headerHeight;

	public Dropdown(Rectangle metric, int headerHeight, List<AbstractComponent> children, string text)
		: base(metric, children)
	{
		_headerHeight = headerHeight;
		TextButton button = new(Rectangle.At(0, 0, metric.Size.X, headerHeight), () => Toggle(!IsOpen), GlobalStyles.DefaultButtonStyle, GlobalStyles.DefaultMiddle, text)
		{
			Depth = 102,
		};
		NestingContext.Add(button);
	}

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		// Close dropdown when children are clicked.
		if (Input.IsButtonPressed(MouseButton.Left) && IsOpen && (Metric with { Y1 = Metric.Y1 + _headerHeight }).Contains(Root.Game.MousePositionWithOffset.RoundToVector2Int32()))
			Toggle(false);
	}
}
