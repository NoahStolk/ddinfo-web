using DevilDaggersInfo.App.Ui.Base.Components;
using Warp.NET.Content;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.LeaderboardList;

public class CriteriaIcon : TooltipIconButton
{
	private static readonly ButtonStyle _buttonStyle = new(Color.Invisible, Color.Invisible, Color.Invisible, 0);

	public CriteriaIcon(IBounds bounds, Action onClick, Texture texture, string tooltipText, Color color)
		: base(bounds, onClick, _buttonStyle, texture, tooltipText, color, color)
	{
	}
}
