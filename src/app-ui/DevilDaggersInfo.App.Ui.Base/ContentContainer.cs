using DevilDaggersInfo.Core.Spawnset;
using Warp.NET.Content;

namespace DevilDaggersInfo.App.Ui.Base;

public record ContentContainer(
	SpawnsetBinary DefaultSpawnset,
	Texture IconDaggerTexture,
	Mesh DaggerMesh,
	Texture DaggerSilverTexture,
	Mesh Skull4Mesh,
	Texture Skull4Texture,
	Mesh Skull4JawMesh,
	Texture Skull4JawTexture,
	Mesh TileMesh,
	Texture TileTexture,
	Mesh PillarMesh,
	Texture PillarTexture,
	Sound SoundJump1,
	Sound SoundJump2,
	Sound SoundJump3,
	Texture PostLut);
