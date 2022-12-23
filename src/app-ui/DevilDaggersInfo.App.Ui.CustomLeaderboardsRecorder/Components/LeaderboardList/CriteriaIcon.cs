using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Extensions;
using DevilDaggersInfo.Core.CriteriaExpression;
using DevilDaggersInfo.Core.CriteriaExpression.Parts;
using DevilDaggersInfo.Types.Core.CustomLeaderboards.Extensions;
using System.Text;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.LeaderboardList;

public class CriteriaIcon : TooltipIconButton
{
	private static readonly ButtonStyle _buttonStyle = new(Color.Invisible, Color.Invisible, Color.Invisible, 0);

	public CriteriaIcon(IBounds bounds, GetCustomLeaderboardCriteria criteria)
		: base(bounds, () => { }, _buttonStyle, criteria.Type.GetTexture(), GetText(criteria), Color.Black, criteria.Type.GetColor())
	{
	}

	private static string GetText(GetCustomLeaderboardCriteria criteria)
	{
		if (!Expression.TryParse(criteria.Expression, out Expression? criteriaExpression))
		{
			// TODO: Log warning.
			return string.Empty;
		}

		StringBuilder sb = new();
		sb.Append(criteria.Type.Display());
		sb.Append(' ');
		sb.Append(criteria.Operator.ShortString());
		sb.Append(' ');

		foreach (IExpressionPart expressionPart in criteriaExpression.Parts)
		{
			switch (expressionPart)
			{
				case ExpressionOperator op:
					sb.Append(op);
					break;
				case ExpressionTarget target:
					sb.Append(target);
					break;
				case ExpressionValue value:
					sb.Append(value.ToDisplayString(criteria.Type));
					break;
			}

			sb.Append(' ');
		}

		return sb.ToString().Trim();
	}
}
