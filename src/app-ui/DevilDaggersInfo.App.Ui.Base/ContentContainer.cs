using DevilDaggersInfo.Core.Spawnset;
using Warp.Content;

namespace DevilDaggersInfo.App.Ui.Base;

public record ContentContainer(
	SpawnsetBinary DefaultSpawnset,
	Mesh Skull4Mesh,
	Texture Skull4Texture,
	Mesh TileMesh,
	Texture TileTexture);