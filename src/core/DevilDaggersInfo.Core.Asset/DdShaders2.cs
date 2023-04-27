namespace DevilDaggersInfo.Core.Asset;

public static class DdShaders2
{
	public static readonly IReadOnlyList<ShaderAssetInfo> All = new List<ShaderAssetInfo>
	{
		new("boid", false),
		new("boid_batch", false),
		new("boid_pop", true),
		new("boid_pop_batch", true),
		new("daggerlevi", false),
		new("debug", false),
		new("default_texture", false),
		new("depth", false),
		new("egg", false),
		new("gib", false),
		new("gib_batch", false),
		new("hand", false),
		new("hand_batch", false),
		new("light", false),
		new("light_batch", false),
		new("light_ring", false),
		new("owlwing", false),
		new("particle", false),
		new("particle2", true),
		new("particle2_batch", true),
		new("particle_decal", false),
		new("particle_decal_batch", true),
		new("post", false),
		new("post_combine", false),
		new("post_final", false),
		new("post_title", false),
		new("shadow", false),
		new("shadow_batch", false),
		new("sorath", false),
		new("sprite", false),
		new("sprite_debug", false),
		new("sprite_terrain", false),
		new("squidarm", false),
		new("squidarm_batch", false),
		new("thorn", false),
		new("tile", false),
		new("tile2", false),
		new("tile_batch", false),
		new("unlit", false),
		new("warp", false),
		new("warp2", false),
		new("warp2_batch", false),
		new("warp_batch", false),
		new("warp3", false),
		new("warpeye", false),
	};
}
