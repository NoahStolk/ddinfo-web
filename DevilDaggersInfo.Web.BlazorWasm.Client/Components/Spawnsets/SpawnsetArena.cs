using DevilDaggersInfo.Core.Spawnset;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Components.Spawnsets;

public partial class SpawnsetArena
{
	[Parameter, EditorRequired] public SpawnsetBinary SpawnsetBinary { get; set; } = null!;
}
