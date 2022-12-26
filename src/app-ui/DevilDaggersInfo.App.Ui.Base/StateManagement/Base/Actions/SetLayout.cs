using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.Base.Actions;

/// <summary>
/// Fires when the layout is changed.
/// </summary>
public record SetLayout(IExtendedLayout Layout) : IAction<SetLayout>
{
	public void Reduce()
	{
		Root.Game.ActiveLayout = Layout;
	}
}
