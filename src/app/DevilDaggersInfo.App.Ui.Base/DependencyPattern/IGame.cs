using DevilDaggersInfo.Core.Versioning;

namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern;

public interface IGame
{
	AppVersion AppVersion { get; }

	float Dt { get; }
	float Tt { get; }
	float MainLoopRate { get; set; }

	TooltipContext? TooltipContext { get; set; }
}
