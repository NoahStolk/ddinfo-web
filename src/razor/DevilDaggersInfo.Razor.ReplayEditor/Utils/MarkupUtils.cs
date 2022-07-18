using DevilDaggersInfo.Core.Replay.Enums;
using DevilDaggersInfo.Razor.ReplayEditor.Extensions;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.ReplayEditor.Utils;

public static class MarkupUtils
{
	public static MarkupString EntityType(int entityId, List<EntityType> entityTypes)
	{
		if (entityId < 0 || entityId >= entityTypes.Count)
			return new("Invalid entity ID");

		EntityType entityType = entityTypes[entityId];
		return new($"{entityId} (<span style='color: {entityType.GetColor().HexCode};'>{entityType.ToDisplayString()}</span>)");
	}
}
