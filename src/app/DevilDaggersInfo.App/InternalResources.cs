using DevilDaggersInfo.App.Engine;
using DevilDaggersInfo.App.Engine.Content;
using Silk.NET.OpenGL;

namespace DevilDaggersInfo.App;

public record InternalResources(
	Shader MeshShader,
	Texture TileHitboxTexture,
	ModelContent TileHitboxModel)
{
	public static InternalResources Create(GL gl)
	{
#if DEBUG
		const string? ddInfoToolsContentRootDirectory = @"..\..\..\..\..\app\DevilDaggersInfo.App.Ui.Base\Content";
#else
		const string? ddInfoToolsContentRootDirectory = null;
#endif
		DecompiledContentFile ddInfoToolsContent = Bootstrapper.GetDecompiledContent(ddInfoToolsContentRootDirectory, "ddinfo");

		ddInfoToolsContent.Shaders.TryGetValue("Mesh", out ShaderContent? meshShaderContent);
		if (meshShaderContent == null)
			throw new InvalidOperationException("Could not find mesh shader.");

		ddInfoToolsContent.Textures.TryGetValue("TileHitbox", out TextureContent? tileHitboxContent);
		if (tileHitboxContent == null)
			throw new InvalidOperationException("Could not find tile hitbox texture.");

		ddInfoToolsContent.Models.TryGetValue("TileHitbox", out ModelContent? tileHitboxModelContent);
		if (tileHitboxModelContent == null)
			throw new InvalidOperationException("Could not find tile hitbox model.");

		Shader meshShader = new(gl, meshShaderContent.VertexCode, meshShaderContent.FragmentCode);
		Texture tileHitbox = new(gl, tileHitboxContent.Pixels, (uint)tileHitboxContent.Width, (uint)tileHitboxContent.Height);
		return new(meshShader, tileHitbox, tileHitboxModelContent);
	}
}
