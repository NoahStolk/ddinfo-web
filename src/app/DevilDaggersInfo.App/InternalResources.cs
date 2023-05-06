using DevilDaggersInfo.App.Engine;
using DevilDaggersInfo.App.Engine.Content;
using Silk.NET.OpenGL;

namespace DevilDaggersInfo.App;

public record InternalResources(
	BlobContent Value,
	Shader MeshShader,
	Texture TileHitboxTexture,
	Texture IconCalendarTexture,
	Texture IconCrosshairTexture,
	Texture IconDaggerTexture,
	Texture IconEggTexture,
	Texture IconEyeTexture,
	Texture IconGemTexture,
	Texture IconHomingTexture,
	Texture IconHomingMaskTexture,
	Texture IconRankTexture,
	Texture IconSkullTexture,
	Texture IconSpiderTexture,
	Texture IconStopwatchTexture,
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

		return new(
			Value: GetBlobContent(ddInfoToolsContent, "Value"),
			MeshShader: GetShader(ddInfoToolsContent, "Mesh"),
			TileHitboxTexture: GetTexture(ddInfoToolsContent, "TileHitbox"),
			IconCalendarTexture: GetTexture(ddInfoToolsContent, "IconCalendar"),
			IconCrosshairTexture: GetTexture(ddInfoToolsContent, "IconCrosshair"),
			IconDaggerTexture: GetTexture(ddInfoToolsContent, "IconDagger"),
			IconEggTexture: GetTexture(ddInfoToolsContent, "IconEgg"),
			IconEyeTexture: GetTexture(ddInfoToolsContent, "IconEye"),
			IconGemTexture: GetTexture(ddInfoToolsContent, "IconGem"),
			IconHomingTexture: GetTexture(ddInfoToolsContent, "IconHoming"),
			IconHomingMaskTexture: GetTexture(ddInfoToolsContent, "IconHomingMask"),
			IconRankTexture: GetTexture(ddInfoToolsContent, "IconRank"),
			IconSkullTexture: GetTexture(ddInfoToolsContent, "IconSkull"),
			IconSpiderTexture: GetTexture(ddInfoToolsContent, "IconSpider"),
			IconStopwatchTexture: GetTexture(ddInfoToolsContent, "IconStopwatch"),
			TileHitboxModel: GetModelContent(ddInfoToolsContent, "TileHitbox"));

		static BlobContent GetBlobContent(DecompiledContentFile content, string name)
		{
			content.Blobs.TryGetValue(name, out BlobContent? blobContent);
			if (blobContent == null)
				throw new InvalidOperationException($"Could not find blob '{name}'.");

			return blobContent;
		}

		Shader GetShader(DecompiledContentFile content, string name)
		{
			content.Shaders.TryGetValue(name, out ShaderContent? shaderContent);
			if (shaderContent == null)
				throw new InvalidOperationException($"Could not find shader '{name}'.");

			return new(gl, shaderContent.VertexCode, shaderContent.FragmentCode);
		}

		Texture GetTexture(DecompiledContentFile content, string name)
		{
			content.Textures.TryGetValue(name, out TextureContent? textureContent);
			if (textureContent == null)
				throw new InvalidOperationException($"Could not find texture '{name}'.");

			return new(gl, textureContent.Pixels, (uint)textureContent.Width, (uint)textureContent.Height);
		}

		static ModelContent GetModelContent(DecompiledContentFile content, string name)
		{
			content.Models.TryGetValue(name, out ModelContent? modelContent);
			if (modelContent == null)
				throw new InvalidOperationException($"Could not find model '{name}'.");

			return modelContent;
		}
	}
}
