using DevilDaggersInfo.Razor.ReplayEditor.Extensions;
using DevilDaggersInfo.Types.Core.Replays;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.ReplayEditor.Utils;

public static class MarkupUtils
{
	public static MarkupString EntityType(int entityId, IReadOnlyList<EntityType> entityTypes)
	{
		if (entityId < 0 || entityId >= entityTypes.Count)
			return new($"{entityId} (<span style='color: red;'>???</span>)");

		EntityType entityType = entityTypes[entityId];
		return new($"{entityId} (<span style='color: {entityType.GetColor().HexCode};'>{entityType.ToDisplayString()}</span>)");
	}
}
